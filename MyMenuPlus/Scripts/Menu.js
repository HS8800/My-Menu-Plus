$("#btn-edit-menu,#btn-new-menu").click(function () {
    window.location.href = "/MenuEditor?content=" + this.dataset.id;
});