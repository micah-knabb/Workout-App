$(document).ready(function () {
    $("#addSet").validate({
        rules: {
            time: {
                required: true,
                digits: true
            },
            distance: {
                required: true,
                digits: true
            },
            intensity: {
                required: true,
                digits: true
            }
        },
        messages: {
            time: {
                required: "Number required, or 0 for empty",
                digits: "Only enter numbers"
            },
            distance: {
                required: "Number required, or 0 for empty",
                digits: "Only enter numbers"
            },
            intensity: {
                required: "Number required, or 0 for empty",
                digits: "Only enter numbers"
            }
        },
    });

    $("#addSet").submit(function (e) {
        e.preventDefault();
        let id = $("#main").data("id");
        let setid = $("#idHere").data("id");
        let time = e.target.elements.time.value;
        let distance = e.target.elements.distance.value;
        let intensity = e.target.elements.intensity.value;

        $.ajax({
            url: alfaUrl + "WorkoutManager/AddEnduranceSet",
            type: "POST",
            dataType: "json",
            data: {
                time: time,
                distance: distance,
                intensity: intensity,
                workoutExercise: id,
                setId: setid
            }
        }).done(function (data) {
            location.reload();
        })
    })
});