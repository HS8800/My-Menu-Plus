
//show create new menu form
$(".btn-create-new").click(function () {
    $("#menu-create-background").css({ "display": "block" });
    $(window).scrollTop(0);
});


//hide create new menu form
$("#menu-create-background,#btn-menu-close").click(function (e) {
    if (e.target !== this)//prevent click event when clicking over menu
        return;

    $("#menu-create-background").css({ "display": "none" });
});

//submit create new menu
$("#menu-create-form").submit(function (e) {
    //Stop Form Redirect
    e.preventDefault();

    //Button Feedback
    $("#btn-menu-create").addClass("btn-state-sent")
    $("#btn-menu-create").text("Creating")
    $(".loading").css({ "display": "block" })
    $("#loading-background").css({ "display": "flex" })

    setTimeout(function () { $(".btn-state-sent").removeClass("btn-state-sent"); $("#btn-menu-create").text("Create Menu"); $(".loading").css({ "display": "none" }); $("#loading-background").css({ "display": "none" }) }, 10000);

    //Send Post
    $.post('/MenuSelection/CreateMenu', { menuName: $("#menu-create-title").val().replace(/(<([^>]+)>)/gi, ""), __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() }).done(function (response) {
        response = JSON.parse(response);
        if (response.response == "success") {

            window.location.href = "/MenuSelection";//Reload page

        } else if (response.response == "failed") {
            $(".errorDisplay").text(response.error)
            $("#loading-background").css({ "display": "none" })
        }
    }).always(function () {
        $(".btn-state-sent").removeClass("btn-state-sent");
        $("#btn-menu-create").text("Create Menu");
        $(".loading").css({ "display": "none" });
    });
    $("#menu-create-title").val("");
});



//Delete Menu
$(".btn-menu-delete").click(function () {

    var menuElement = $(this)[0];
    if (confirm("You are about to permanently delete the menu " + menuElement.parentElement.parentElement.childNodes[1].innerText + ",\nare you sure?")) {
        //Send Post

        $("#loading-background").css({ "display": "flex" })
        $.post('/MenuSelection/DeleteMenu', { menuID: menuElement.dataset.id, __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()}).done(function (response) {
            response = JSON.parse(response);
            if (response.response == "success") {
                $(menuElement.parentElement.parentElement).remove();//Delete visual      
            } else if (response.response == "failed") {
                $(".errorDisplay").text(response.error)
            }
           
        }).always(function () {
            $("#loading-background").css({ "display": "none" })
        });
    }
 
});


//Select Menu
$('#menu-container > div').click(function (e) {
    if (e.target.classList.contains("btn-menu-delete") || e.target.classList.contains("menu-btn-container"))//prevent this click event when clicking delete
        return;
    
    var menuID;//find menu id
    if (e.target.classList.contains("fa-plus-square")) {
        menuID = e.target.parentElement.childNodes[0].childNodes[0].dataset.id;
    } else if (e.target.classList.contains("select-thumbnail")) {
        menuID = e.target.parentElement.childNodes[0].childNodes[0].dataset.id;
    } else {
        menuID = e.target.childNodes[0].childNodes[0].dataset.id;
    }

    $("#loading-background").css({ "display": "flex" })
    window.location.href = "/Menu?content="+menuID;//Go to menu


});


setTimeout(function () {//after 2 seconds cancel animation fade in delay to avoid users with lots of menus having to wait
    $("#menu-container > div").attr("data-aos-delay", "0");
}, 2000);
