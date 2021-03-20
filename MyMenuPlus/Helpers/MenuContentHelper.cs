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


            //Check image is valid
            if (menuImage.Length > 800000) {
                return (false, "Image is to large, please use an image less than 600kbs");
            } else if (menuData.Length > 16777215) {
                return (false, "Your menu is to large, please reduce the number of items to save");
            }


            //Create ids for menu items
            dynamic menuDataJSON = JsonConvert.DeserializeObject(menuData);
            int itemID = 1;
            //Loop through menu section
            dynamic sections = menuDataJSON.sections;
            for (int s = 0; s < sections.Count; s++)
            {
                //Add id to each menu item
                dynamic sectionItems = sections[s].sectionItems;
                for (int si = 0; si < sectionItems.Count; si++)
                {

                    try//In event where the price is "" or can't be a decimal this item will be skipped
                    {
                        sectionItems[si].price = Convert.ToDecimal(sectionItems[si].price).ToString("C").Substring(1);//format money
                        sectionItems[si].id = itemID++;
                    }
                    catch { }
                }
            }

            //Replace old menu items with new items with ids 
            menuData = JsonConvert.SerializeObject(menuDataJSON);

            //Save to database
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

        internal static (bool success, string title, string tags, string sections, string bannerImage, string menuNavigaton) createMenuComponents(int menuID, int accountID)
        {

            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL getMenuContent(@menuID)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@menuID", menuID);
            

                StringBuilder sectionBuilder = new StringBuilder();
                StringBuilder tagBuilder = new StringBuilder();
                StringBuilder menuNavigaton = new StringBuilder();

                string title = "";
                string menuImage = "";

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    if (Convert.ToString(reader["menuData"]) == "initial") {
                        return (true, "Welcome to your new menu!", "<div>An empty template</div>", $@"

                            <div style=' padding: 20px;'>                       
                            <h3 style='text-align: center;'>Your menu looks a bit empty at the moment</h3>

                            <button data-id='{menuID}' class='btn' id='btn-new-menu' style='
                                margin: 0px auto 93px auto;
                                display: block;
                                background-color: #cc3333;
                                font-weight: 700;
                            '>Start Creating</button>
    
                            </div>

                            <style>
                                #basket-container{{display:none}}
                                #main-container{{width:100%}}
                            </style>

                        ", "https://buckeyevillagemansfield.com/wp-content/uploads/2015/10/placeholder-pattern.jpg", "");
                    }

                    dynamic menuData = JsonConvert.DeserializeObject(Convert.ToString(reader["menuData"]));

                    //Get menu title
                    title = menuData.title;

                    //Get menu banner image
                    menuImage = Convert.ToString(reader["menuImage"]);
                    if (menuImage == "")
                    {
                        menuImage = "/Media/Placeholder-pattern.jpg";
                    }

                    //Generate menu tags
                    dynamic tags = menuData.tags;
                    for (int i = 0; i < tags.Count; i++)
                    {

                        if (i % 2 != 0)
                        {//if odd add dot in-between tag
                            tagBuilder.Append("<span>•</span>");
                        }

                        tagBuilder.Append("<span>");
                        tagBuilder.Append(tags[i]);
                        tagBuilder.Append("</span>");
                    }





                    //Create menu sections
                    dynamic sections = menuData.sections;
                    for (int s = 0; s < sections.Count; s++)
                    {
                      
                        dynamic section = sections[s];
                        StringBuilder sectionItemElements = new StringBuilder();


                        //Build menu items
                        dynamic sectionItems = section.sectionItems;
                        for (int si = 0; si < sectionItems.Count; si++)
                        {
             

                            if (Convert.ToString(sectionItems[si].price) != "0.00")
                            {
                                sectionItemElements.Append(createMenuItem(
                                    Convert.ToString(sectionItems[si].name),
                                    Convert.ToString(sectionItems[si].description),
                                    Convert.ToString(sectionItems[si].price),
                                    Convert.ToInt32(sectionItems[si].id)
                                ));
                            }

                                //Convert.ToBoolean(sectionItems[si].isVegetarian),
                                //Convert.ToBoolean(sectionItems[si].isSpicy)
                        }


                        menuNavigaton.Append("<div>"+ section.title + "</div>");
                        sectionBuilder.Append(createMenuSection(
                             Convert.ToString(section.title), 
                            sectionItemElements.ToString()
                        ));

                    }
                }

                return (true, title, tagBuilder.ToString(), sectionBuilder.ToString(), menuImage, menuNavigaton.ToString());
            }
            catch (MySqlException ex)
            {
                return (false, "", "", "", "", "");
            }
        }

     
        internal static (bool success, string title, string tags, string sections, string bannerImage) createMenuEditorComponents(int menuID, int accountID)
        {
         
            MySqlConnection connection = new MySqlConnection(Helpers.ConfigHelper.connectionString);
            try
            {
                connection.Open();
                string query = "CALL getMenuContent(@menuID)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@menuID", menuID);
          

                StringBuilder sectionBuilder = new StringBuilder();
                StringBuilder tagBuilder = new StringBuilder();
                string title = "";
                string menuImage = "";

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    
                    if (Convert.ToString(reader["menuData"]) == "initial")
                    {
                        return (true, "", "", "", "https://buckeyevillagemansfield.com/wp-content/uploads/2015/10/placeholder-pattern.jpg");
                    }


                    int unquieID = 0;
                    dynamic menuData = JsonConvert.DeserializeObject(Convert.ToString(reader["menuData"]));

                    //Get menu title
                    title = menuData.title;

                    //Get menu banner image
                    menuImage = Convert.ToString(reader["menuImage"]);
                    if (menuImage == "") {
                        menuImage = "https://buckeyevillagemansfield.com/wp-content/uploads/2015/10/placeholder-pattern.jpg";
                    }

                    //Generate menu tags
                    dynamic tags = menuData.tags;

                    for (int i = 0; i < tags.Count; i++)
                    {
                        tagBuilder.Append(createEditorTag(Convert.ToString(tags[i])));                     
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
                        <i class='fas fa-trash-alt' onclick='$(this.parentNode.parentNode.parentNode)[0].remove();EditorChanges()'></i>
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
                    <i class='fas fa-trash-alt' onclick='$(this.parentNode.parentNode.parentNode)[0].remove();EditorChanges()'></i>
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
                        <input class='item-price' oninput='setAttValue(this)' placeholder='Price' onchange='this.value = currency.format(this.value).replace(/[£]/g, """")' value='{price}' type='number' min='0.01' step='0.01'>
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

        internal static string createEditorTag(string tagName) { 
        
            string tag = $@"
            <div draggable = 'true' class='editor-section section-item section-tag ui-sortable-handle' style=''>
                <div class='editor-section-handle'>
                    <ul class='editor-section-controls tags'>
                        <i class='fas fa-trash-alt' onclick='$(this.parentNode.parentNode.parentNode)[0].remove();EditorChanges()'></i>
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

        internal static string createMenuItem(string itemName, string itemDescription, string itemPrice, int itemID)
        {

            string item = $@"
                <tr data-itemID='{itemID}'>
	                <td>
		                <div class='item-name'>{itemName}</div>
		                <div class='item-description'>{itemDescription}</div>
	                </td>
	                <td class='item-right'>
		                <span class='item-price'>£{itemPrice}</span><button class='item-btn item-add'>+</button>
	                </td>
                </tr>
            ";

            return item;

        }

        internal static string createMenuSection(string sectionName, string itemDescription)
        {

            string item = $@"
                <h1>{sectionName}</h1>
                <table>
                <tbody>
                    {itemDescription}
                </tbody>
                </table>
            ";

            return item;

        }


    }
}