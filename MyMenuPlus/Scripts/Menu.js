$("#btn-edit-menu,#btn-new-menu").click(function () {
    $("#loading-background").css({ "display": "flex" })
    window.location.href = "/MenuEditor?content=" + this.dataset.id;
});

$("#btn-notOpen-close").click(function () {
    $("#notOpen-background").hide()
});

