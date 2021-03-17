using MyMenuPlus.Helpers;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;


namespace MyMenuPlus.Helpers
{
    internal sealed class MenuContentHelper
    {

        internal static (bool success, string details) updateMenu(int menuID, int accountID, string menuData, string menuImage)
        {

            if (menuImage.Length > 800000) {
                return (false, "Image is to large, please use an image less than 600kbs");
            } else if (menuData.Length > 16777215) {
                return (false, "Your menu is to large, please reduce the number of items to save");
            }

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL updateMenu(@accountID,@ownerID,@menuData,@menuImage)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@accountID", menuID);
                command.Parameters.AddWithValue("@ownerID", accountID);
                command.Parameters.AddWithValue("@menuData", menuData);
                command.Parameters.AddWithValue("@menuImage", menuImage);

                command.ExecuteNonQuery();
                return (true, "Menu updated");
            }
            catch (MySqlException ex)
            {
                return (false, "an internal error occured");
            }
        }


        internal static (bool success, string details) DeleteMenu(int menuID, int accountID)
        {
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL deleteMenu(@menuID,@accountID)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@menuID", menuID);
                command.Parameters.AddWithValue("@accountID", accountID);

                command.ExecuteNonQuery();
                return (true, "Menu deleted");
            }
            catch (MySqlException ex)
            {
                return (false, "an internal error occured");
            }
        }


        internal static (bool success, string title, string tags, string sections, string bannerImage) createMenuComponents(int menuID, int accountID)
        {
         
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL getMenuContent(@menuID,@accountID)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@menuID", menuID);
                command.Parameters.AddWithValue("@accountID", accountID);

                StringBuilder sectionBuilder = new StringBuilder();
                StringBuilder tagBuilder = new StringBuilder();
                string title = "";
                string menuImage = "";

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int unquieID = 0;
                    dynamic menuData = JsonConvert.DeserializeObject(Convert.ToString(reader["menuData"]));

                    //Get menu title
                    title = menuData.title;

                    //Get menu banner image
                    menuImage = Convert.ToString(reader["menuImage"]);



                    //Generate menu tags
                    //dynamic tags = menuData.tags;
                    //for (int i = 0; i < tags.Count; i++) {

                    //    if (i%2 != 0){//if odd add dot in-between tag
                    //        tagBuilder.Append("<span>•</span>");
                    //    }

                    //    tagBuilder.Append("<span>");
                    //    tagBuilder.Append(tags[i]);
                    //    tagBuilder.Append("</span>");
                    //}


                    //Generate menu tags
                    dynamic tags = menuData.tags;

                    for (int i = 0; i < tags.Count; i++)
                    {
                        tagBuilder.Append(createTag(Convert.ToString(tags[i])));                     
                    }



                    //Create menu editor dections
                    dynamic sections = menuData.sections;
                    for (int s = 0; s < sections.Count; s++)
                    {
                        unquieID++;//This is used to tell each section apart when moving them around on the page
                        dynamic section = sections[s];
                        StringBuilder sectionItemElements = new StringBuilder();
                       

                        //Build menu items
                        dynamic sectionItems = section.sectionItems;
                        for (int si = 0; si < sectionItems.Count; si++)
                        {
                            
                            sectionItemElements.Append(createEditorItem(
                                Convert.ToString(sectionItems[si].name),
                                Convert.ToString(sectionItems[si].description),
                                Convert.ToString(sectionItems[si].price),
                                Convert.ToBoolean(sectionItems[si].isVegetarian),
                                Convert.ToBoolean(sectionItems[si].isSpicy)
                             ));

                        }

                        sectionBuilder.Append(createEditorSection(
                            Convert.ToString(section.title), 
                            sectionItemElements.ToString(), 
                            unquieID
                        ));
                        
                    }
                }

                return (true, title, tagBuilder.ToString(), sectionBuilder.ToString(), menuImage);
            }
            catch (MySqlException ex)
            {
                return (false, "", "", "", "");
            }
        }


        internal static (bool success, string details) CreateMenu(string menuName, int accountID) {
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL createMenu(@accountID,@menuName)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@menuName", menuName);
                command.Parameters.AddWithValue("@accountID", accountID);

                command.ExecuteNonQuery();
                return (true, "New menu created");
            }
            catch (MySqlException ex)
            {
                return (false, "an internal error occured");
            }
        }


        internal static string MenuThumbnails(int accountID)
        {
           
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString); 
            try
            {
                connection.Open();             
                string query = "CAll getMenus(@accountID)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@accountID", accountID);


                StringBuilder thumbnailBuilder = new StringBuilder();
                MySqlDataReader reader = command.ExecuteReader();

                int count = 1;
                while (reader.Read())
                {

             
                    thumbnailBuilder.Append("<div data-aos='fade-up' data-aos-delay='");

                    thumbnailBuilder.Append(count*100);//animation fade in delay
                    thumbnailBuilder.Append("'>");
                    thumbnailBuilder.Append("<div class='menu-btn-container' ><i class='fas fa-trash-alt btn-menu-delete' data-id='");
                    thumbnailBuilder.Append(reader["id"]);
                    thumbnailBuilder.Append("' btn-menu-delete' ></i></div>");
                    thumbnailBuilder.Append("<div class='section-title title-bold select-thumbnail'>");
                    thumbnailBuilder.Append(Convert.ToString(reader["title"]).Length > 13 ? Convert.ToString(reader["title"]).Substring(0, 10)+"..." : reader["title"]);//title LIMIT to no more than 13
                    thumbnailBuilder.Append("</div>");
                    thumbnailBuilder.Append("<i class='fa fa-plus-square'></i>");
                    thumbnailBuilder.Append("</div>");

                    count++;

                }

                return thumbnailBuilder.ToString();
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return "<h3>Your menus could not be loaded at this time<h3>";
            }

        }

        internal static string createEditorSection(string title,string sectionItems,int unquieID) {

            string editorSection = $@"
            <div class='editor-section'>
                <div class='editor-section-handle'>
                    <ul class='editor-section-controls toolbar-sections'>
                        <i class='fas fa-trash-alt' onclick='$(this.parentNode.parentNode.parentNode)[0].remove()'></i>
                        <i class='fas fa-caret-up' onclick='moveSectionUp(this)'></i>
                        <i class='fas fa-caret-down' onclick='moveSectionDown(this)'></i>
                    </ul>
                </div>
            <div class='editor-section-content'>

            <div class='editor-section section-title'>
                <div class='editor-section-handle'></div>
                <div class='editor-section-content'>
                    <input oninput = 'setAttValue(this)' class='section-title-input' value='{title}' placeholder='Title'>
                </div>
            </div>

            <div class='editor-section-items' id='{unquieID}'>{sectionItems}</div>

            <div class='editor-add'>Add Item</div>
            </div>
            </div>";

            return editorSection;

        }


        internal static string createEditorItem(string name,string description, string price, bool isVegetarian, bool isSpicy) {

            string item = $@"
            <div draggable = 'true' class='editor-section section-item ui-sortable-handle'>
                <div class='editor-section-handle'>
    
                <ul class='editor-section-controls toolbar-items'>
                    <i class='fas fa-trash-alt' onclick='$(this.parentNode.parentNode.parentNode)[0].remove()'></i>
                    <i class='fas fa-caret-up' onclick='moveItemSectionUp(this)'></i>
                    <i class='fas fa-caret-down' onclick='moveItemSectionDown(this)'></i>
                </ul>

                </div>
                <div class='editor-section-content'>
                    <div>
                        <input class='item-name' value='{name}' oninput='setAttValue(this)' placeholder='Item Name'>
                        <textarea class='item-description' value='' placeholder='Item Description' rows='4' cols='50' oninput='setAttValue(this)'>{description}</textarea>
                    </div>
                    <div>
                        <input class='item-price' oninput='setAttValue(this)' placeholder='Price' onchange='this.value = currency.format(this.value).replace(/[£]/g, \'\')' value='{price}' type='number' min='0.01' step='0.01'>
                        Tags<br>
                        <input class='item-veg' oninput='setChkValue(this)' type='checkbox' value='Vegetarian' {(isVegetarian ? "checked" : "")}>
                        <label for='Vegetarian'>Vegetarian</label><br>
                        <input class='item-spicy' oninput='setChkValue(this)' type='checkbox' value='Spicy' {(isSpicy ? "checked" : "")}>
                        <label for='Vegetarian'>Spicy</label><br>
                    </div>
                </div>
            </div>";
            return item;
        }

        internal static string createTag(string tagName) { 
        
            string tag = $@"
            <div draggable = 'true' class='editor-section section-item section-tag ui-sortable-handle' style=''>
                <div class='editor-section-handle'>
                    <ul class='editor-section-controls tags'>
                        <i class='fas fa-trash-alt' onclick='$(this.parentNode.parentNode.parentNode)[0].remove()'></i>
                        <i class='fas fa-caret-left controls-half' onclick='moveTagLeft(this)'></i>
                        <i class='fas fa-caret-right controls-half' onclick='moveTagRight(this)' style='float: right;'></i>
                    </ul>
                </div>
                <div class='editor-section-content'>
                <input placeholder='Tag' oninput='setAttValue(this)' value='{tagName}' style='height: 31px;'>
                </div>
            </div>";

            return tag;

        }


    }
}