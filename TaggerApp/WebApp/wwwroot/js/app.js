console.log("app.js run");
document.addEventListener('DOMContentLoaded', function () {
    console.log("app.js dom loaded");
    InitMasonry();
});

function InitMasonry() {
    console.log("init masonry");
    var grid = document.querySelector('#masonry-grid');

    if (grid == null) return;

    var msnry = new Masonry(grid, {
        itemSelector: '.grid-item',
        //columnWidth: '.grid-sizer',
        percentPosition: true
    });
}