
//jquery ui draggable sections
$(function () {
    $(".editor-section-items").sortable({
        out: function () {
            EditorChanges()
        }
    });
    $(".editor-section-items").disableSelection();

    $(".editor-section-tags").sortable({
        out: function () {
            EditorChanges()
        }
    });

    $(".editor-section-tags").disableSelection();  
});



//These functions set the inputs state within the html to they persist when copyed
function setAttValue(input) {
    $(input).attr("value", input.value)
}

function setChkValue(input) {

    if (input.checked) {
        input.setAttribute("checked", "");
    } else {
        input.removeAttribute("checked");
    } 
}



//Item section structure
const itemSection = `<div draggable="true" class="editor-section section-item">
    <div class="editor-section-handle">
    
    <ul class="editor-section-controls toolbar-items">
        <i class="fas fa-trash-alt" onclick="$(this.parentNode.parentNode.parentNode)[0].remove()"></i>
        <i class="fas fa-caret-up" onclick="moveItemSectionUp(this)"></i>
        <i class="fas fa-caret-down" onclick="moveItemSectionDown(this)"></i>
    </ul>

    </div>
    <div class="editor-section-content">
        <div>
            <input oninput="setAttValue(this)" placeholder="Item Name">
            <textarea value="" placeholder="Item Description" rows="4" cols="50" oninput="setAttValue(this)"></textarea>
        </div>
        <div>
            <input oninput="setAttValue(this)" placeholder="Price" onchange="this.value = currency.format(this.value).replace(/[£]/g, '')" type="number" min="0.01" step="0.01">
            Tags<br>
            <input oninput="setChkValue(this)" type="checkbox" value="Vegetarian">
            <label for="Vegetarian">Vegetarian</label><br>
            <input oninput="setChkValue(this)" type="checkbox" value="Spicy">
            <label for="Vegetarian">Spicy</label><br>
        </div>
    </div>
</div>`

//detect changes to items when dragged
$(".editor-add:not(.add-section)").click(function () {
    $(this.parentElement.children[1]).append(itemSection)
    EditorChanges();
});


//section structure
const Section = `
        <div class="editor-section">
            <div class="editor-section-handle">
                <ul class="editor-section-controls toolbar-sections">
                    <i class="fas fa-trash-alt" onclick="$(this.parentNode.parentNode.parentNode)[0].remove()"></i>
                    <i class="fas fa-caret-up" onclick="moveSectionUp(this)"></i>
                    <i class="fas fa-caret-down" onclick="moveSectionDown(this)"></i>
                </ul>
            </div>
            <div class="editor-section-content">

                <div class="editor-section section-title">
                    <div class="editor-section-handle"></div>
                    <div class="editor-section-content">
                        <input oninput="setAttValue(this)" class="section-title-input" placeholder="Title">

                    </div>
                </div>


                <div class="editor-section-items">
                   
                </div>
                <div class="editor-add">Add Item</div>
            </div>
        </div>`;


//tag structure
const Tag = `
<div draggable="true" class="editor-section section-item section-tag ui-sortable-handle" style="">
    <div class="editor-section-handle">
        <ul class="editor-section-controls tags">
            <i class="fas fa-trash-alt" onclick="$(this.parentNode.parentNode.parentNode)[0].remove()"></i>
            <i class="fas fa-caret-left controls-half" onclick="moveTagLeft(this)" ></i>
            <i class="fas fa-caret-right controls-half" onclick="moveTagRight(this)" style="float: right;"></i>
        </ul>
    </div>
    <div class="editor-section-content">
    <input placeholder="Tag" oninput="setAttValue(this)" style="height: 31px;">
    </div>
</div>`;


var tempID = 1;
//detect changes when a section is created
$(".add-section").click(function () {
    $("#section-container").append(Section);

    
    $("#section-container > div > .editor-section-content > .editor-section-items").last().prop({ "id": tempID++ })

    $(".editor-add:not(.add-section)").last().click(function () {
        $(this.parentElement.children[1]).append(itemSection)

        $(".editor-section-items").sortable({
            connectWith: '.editor-section-items',
            out: function () {
                EditorChanges()
            }
        });
        $(".editor-section-items").disableSelection();  
    });
  
    EditorChanges();
});




//Tag Section Left Right Controls
function moveTagLeft(button) {
    var tag = button.parentNode.parentNode.parentNode;

    if ($(tag).index() != 0) {
        //Create New Element
        $(".editor-section-tags > div:nth-child(" + ($(tag).index()) + ")").before($(tag)[0].outerHTML);

        //Remove Old Element
        $(tag).remove();
    }

}


function moveTagRight(button) {
    var tag = button.parentNode.parentNode.parentNode;

    if ($(tag).index() != $(".editor-section-tags > div").length - 1) {

        //Create New Element
        $($(tag)[0].outerHTML).insertAfter($(".editor-section-tags > div:nth-child(" + ($(tag).index() + 2) + ")"));

        //Remove Old Element
        $(tag).remove();
    }
}


//Item Section Up Down Controls
function moveItemSectionDown(button) {
    var item = button.parentNode.parentNode.parentNode;
    var SectionID = item.parentNode.id;
    


    if ($(item).index() != $("#" +SectionID+" > div").length - 1) {

        //Create New Element
        $($(item)[0].outerHTML).insertAfter($("#" + SectionID+" > div:nth-child(" + ($(item).index() + 2) + ")"));

        //New Element
        var newElement = $("#" + SectionID +" > div:nth-child(" + ($(item).index() + 3) + ")");

        //Update Item Description
        var Description = newElement[0].children[1].children[0].children[1];
        Description.innerText = $(Description).attr("value");

        //Reformat Price
        var Price = newElement[0].children[1].children[1].children[0];
        Price.value = currency.format(Price.value).replace(/[£]/g, '');


        //Remove Old Element
        $(item).remove();
    }
}


function moveItemSectionUp(button) {
    var item = button.parentNode.parentNode.parentNode;
    var SectionID = item.parentNode.id;

    if ($(item).index() != 0) {
         //Create New Element
        $("#" + SectionID + " > div:nth-child(" + ($(item).index()) + ")").before($(item)[0].outerHTML);

        //New Element
        var newElement = $("#" + SectionID +" > div:nth-child(" + ($(item).index() - 1) + ")");

        //Update Item Description
        var Description = newElement[0].children[1].children[0].children[1];
        Description.innerText = $(Description).attr("value");

        //Reformat Price
        var Price = newElement[0].children[1].children[1].children[0];
        Price.value = currency.format(Price.value).replace(/[£]/g, '');

        //Remove Old Element
        $(item).remove();
    }
 
}

//Section Up Down Controls
function moveSectionDown(button) {
    var item = button.parentNode.parentNode.parentNode;

   
    if ($(item).index() != $("#section-container > div").length - 1) {

        //Create New Element
        $($(item)[0].outerHTML).insertAfter($("#section-container > div:nth-child(" + ($(item).index() + 2) + ")"));

        //New Element
        var newElement = $("#section-container > div:nth-child(" + ($(item).index() + 3) + ")");

    
        $(newElement[0].children[1].children[2]).click(function () {
            $(this.parentElement.children[1]).append(itemSection)

            $(".editor-section-items").sortable({
                connectWith: '.editor-section-items',
                out: function () {
                    EditorChanges()
                }
            });
            $(".editor-section-items").disableSelection();
        });

        EditorChanges();

        //Remove Old Element
        $(item).remove();
    }
}


function moveSectionUp(button) {
    var item = button.parentNode.parentNode.parentNode;

    if ($(item).index() != 0) {
        //Create New Element
        $("#section-container > div:nth-child(" + ($(item).index()) + ")").before($(item)[0].outerHTML);

        //New Element
        var newElement = $("#section-container > div:nth-child(" + ($(item).index() - 1) + ")");
        
        $(newElement[0].children[1].children[2]).click(function () {
            $(this.parentElement.children[1]).append(itemSection)

            $(".editor-section-items").sortable({
                connectWith: '.editor-section-items',
                out: function () {
                    EditorChanges()
                }
            });
            $(".editor-section-items").disableSelection();
        });

        EditorChanges();

        //Remove Old Element
        $(item).remove();
    }

}



$(".editor-add-tag").click(function () {
    $(".editor-section-tags").append(Tag);
    EditorChanges();
});

function EditorChanges() {
    $("#toolbar-uptodate").css({ "display": "none" })
    $("#toolbar-save").css({ "display": "inline" })
}

$("body").keypress(function () {
    EditorChanges();
});

$(".section-file-import").change(function () {
    console.log(this.files[0]);
});


const input = $("#section-file-import")[0];
input.addEventListener('change', function (e) {

    console.log(input.files[0].type)

    if (input.files[0].type == "image/jpeg" || input.files[0].type == "image/png") {
        var reader = new FileReader();
        reader.readAsDataURL(input.files[0]);

        reader.onload = function () {    
            console.log(reader.result);

            $("#section-file-import").css({ "background-image": "url('" + reader.result + "')" })
            EditorChanges()
        };

        reader.onerror = function (error) {
            console.log("Error: ", error);
            alert("Oh no looks like something went wrong.");
        };

    } else {
        $(".section-file-import")[0].value = "";
        alert("image pust be png or jpeg");
    }

});
