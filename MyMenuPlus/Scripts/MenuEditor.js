
//jquery ui draggable sections
$(function () {
    $(".editor-section-items").sortable();
    $(".editor-section-items").disableSelection();   
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
});

const Section = `
        <div class="editor-section">
            <div class="editor-section-handle"></div>
            <div class="editor-section-content">

                <div class="editor-section section-title">
                    <div class="editor-section-handle"></div>
                    <div class="editor-section-content">

                        Section Title
                        <input class="section-title-input" placeholder="Enter Title Here">

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

        $(".editor-section-items").sortable({ connectWith: '.editor-section-items'});
        $(".editor-section-items").disableSelection();  
    });
});

