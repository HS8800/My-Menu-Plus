
//show create new menu form
$(".btn-create-new").click(function () {
    $("#menu-create-background").css({ "display": "block" });
});


//hide create new meu form
$("#menu-create-background,#btn-menu-close").click(function (e) {
    if (e.target !== this)//prevent click event when clicking over menu
        return;

    $("#menu-create-background").css({ "display": "none" });
});


