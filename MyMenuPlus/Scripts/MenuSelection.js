
//show create new menu form
$(".btn-create-new").click(function () {
    $("#menu-create-background").css({ "display": "block" });
    $(window).scrollTop(0);
});


//hide create new meu form
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

    setTimeout(function () { $(".btn-state-sent").removeClass("btn-state-sent"); $("#btn-menu-create").text("Create Menu"); $(".loading").css({ "display": "none" }) }, 10000);

    //Send Post
    $.post('/MenuSelection/CreateMenu', { menuName: $("#menu-create-title").val(), __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() }).done(function (response) {
        response = JSON.parse(response);
        if (response.response == "success") {
            
            newNotification("Created")
            window.location.href = "/MenuSelection";//Reload page

        } else if (response.response == "failed") {
            $(".errorDisplay").text(response.error)
        }
    }).always(function () {
        $(".btn-state-sent").removeClass("btn-state-sent");
        $("#btn-menu-create").text("Create Menu");
        $(".loading").css({ "display": "none" });
    });
    $("#menu-create-title").val("");
});

setTimeout(function () {//after 2 seconds cancel animation fade in delay to avoid users with lots of menus having to wait
    $("#menu-container > div").attr("data-aos-delay", "0");
}, 2000);
