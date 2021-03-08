
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


const Tag = `
<div draggable="true" class="editor-section section-item section-tag ui-sortable-handle" style="">
    <div class="editor-section-handle" style=""></div>
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

