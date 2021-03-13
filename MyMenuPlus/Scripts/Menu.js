$("#btn-edit-menu").click(function () {
    window.location.href = "/MenuEditor?content=" + this.dataset.id;
});