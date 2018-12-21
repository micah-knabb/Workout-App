$(document).ready(function () {
    $(".list-group-item").on("click", function (event) {
        console.log(event.target);
        event.target.value
    })
})    