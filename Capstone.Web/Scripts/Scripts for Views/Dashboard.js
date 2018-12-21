$(document).ready(function () {
    $(".active").removeClass("active");
    $(".dashboardNav").addClass("active");
    
    $(".deleteWorkoutBtn").on("click", function (e) {
        let WorkoutId = e.currentTarget.id

        if (confirm('Are you sure?')) {
            $.ajax({
                url: alfaUrl + "WorkoutManager/Delete",
                type: "DELETE",
                dataType: "json",
                data: {
                    id: WorkoutId
                }
                }).done(function (data) {
                    if (data == true) {
                        $("#" + WorkoutId).remove(); 
                    }
                })
        }
    });

    $(".card-body").on("click", function (e) {
        let parent = $(e.target).parent();
        let id =parent.attr("id");
        location.assign(alfaUrl+"GoMode/Start/"+id);
    })
    $(".card-body *").on("click", function (e) {
        let parent = $(e.target).parent();
        let grandparent = parent.parent();
        let id = grandparent.attr("id");
        e.stopPropagation()
        location.assign(alfaUrl + "GoMode/Start/" + id);
    });
});
