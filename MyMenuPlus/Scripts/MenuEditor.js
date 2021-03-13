
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


const itemSection = `<div draggable="true" class="editor-section section-item">
    <div class="editor-section-handle"></div>
    <div class="editor-section-content">
        <div>
            <input placeholder="Item Name">
                <textarea placeholder="Item Description" rows="4" cols="50"></textarea>
        </div>
        <div>
            <input placeholder="Price" onchange="this.value = currency.format(this.value).replace(/[£]/g, '')" type="number" min="0.01" step="0.01">
            Tags<br>
            <input type="checkbox" value="Vegetarian">
            <label for="Vegetarian">Vegetarian</label><br>
            <input type="checkbox" value="Spicy">
            <label for="Vegetarian">Spicy</label><br>
        </div>
    </div>
</div>`

$(".editor-add:not(.add-section)").click(function () {
    $(this.parentElement.children[1]).append(itemSection)
    EditorChanges();
});

const Section = `
        <div class="editor-section">
            <div class="editor-section-handle"></div>
            <div class="editor-section-content">

                <div class="editor-section section-title">
                    <div class="editor-section-handle"></div>
                    <div class="editor-section-content">
                        <input class="section-title-input" placeholder="Title">

                    </div>
                </div>


                <div class="editor-section-items">
                   
                </div>
                <div class="editor-add">Add Item</div>
            </div>
        </div>`;

$(".add-section").click(function () {
    $("#section-container").append(Section);

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


//function insertAtIndex(i) {
//    if (i === 0) {
//        $("#controller").prepend("<div>okay things</div>");
//        return;
//    }


//    $("#controller > div:nth-child(" + (i) + ")").after("<div>great things</div>");
//}

function moveTagLeft(button) {
    var tag = button.parentNode.parentNode.parentNode;
    if ($(tag).index() != 0) {

        $(".editor-section-tags > div:nth-child(" + ($(tag).index()) + ")").before(`
        <div draggable="true" class="editor-section section-item section-tag ui-sortable-handle" style="">
            <div class="editor-section-handle">
                <ul class="editor-section-controls tags">
                    <i class="fas fa-trash-alt" onclick="$(this.parentNode.parentNode.parentNode)[0].remove()"></i>
                    <i class="fas fa-caret-left controls-half" onclick="moveTagLeft(this)"></i>
                    <i class="fas fa-caret-right controls-half" onclick="moveTagRight(this)" style="float: right;"></i>
                </ul>
            </div>
            <div class="editor-section-content">
            <input placeholder="Tag" style="height: 31px;" value="`+ tag.children[1].children[0].value + `">
            </div>
        </div>
    `);
        $(tag).remove();
    }
}

function moveTagRight(button) {
    var tag = button.parentNode.parentNode.parentNode;

    if ($(tag).index() != $(".editor-section-tags > div").length-1) {

       $(`<div draggable="true" class="editor-section section-item section-tag ui-sortable-handle" style="">
            <div class="editor-section-handle">
                <ul class="editor-section-controls tags">
                    <i class="fas fa-trash-alt" onclick="$(this.parentNode.parentNode.parentNode)[0].remove()"></i>
                    <i class="fas fa-caret-left controls-half" onclick="moveTagLeft(this)"></i>
                    <i class="fas fa-caret-right controls-half" onclick="moveTagRight(this)" style="float: right;"></i>
                </ul>
            </div>
            <div class="editor-section-content">
            <input placeholder="Tag" style="height: 31px;" value="`+ tag.children[1].children[0].value + `">
            </div>
        </div>
    `).insertAfter($(".editor-section-tags > div:nth-child(" + ($(tag).index()+2) + ")"));
       $(tag).remove();
    }
}

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
    <input placeholder="Tag" style="height: 31px;">
    </div>
</div>`;

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
