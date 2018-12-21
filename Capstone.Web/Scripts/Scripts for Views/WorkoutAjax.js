$(document).ready(function () {
    $(".downLink").on("click", function (e) {
        let orderId = e.currentTarget.id
        if (orderId > 1) {
            $.ajax({
                url: alfaUrl + "WorkoutManager/ExerciseDown",
                type: "PUT",
                dataType: "json",
                data: {
                    Order: orderId,
                }
            }).done(function (data) {
                location.reload();
            })
        }
    });

    $(".upLink").on("click", function (e) {
        let orderId = e.currentTarget.id;
        let max = $("#workoutBox").data("exercises")
        if (orderId < max) {
            $.ajax({
                url: alfaUrl + "WorkoutManager/ExerciseUp",
                type: "PUT",
                dataType: "json",
                data: {
                    Order: orderId,
                }
            }).done(function (data) {
                location.reload();
            })
        }
    });

    $(".deleteExercBtn").on("click", function (e) {
        let ExerciseId = e.currentTarget.dataset.id

        if (confirm('Are you sure?')) {

            $.ajax({
                url: alfaUrl + "WorkoutManager/DeleteExercise",
                type: "DELETE",
                dataType: "json",
                data: {
                    id: ExerciseId
                }
            }).done(function (data) {
                location.reload();
            })
        }
    });

    $(".details").one("click", function (e) {
        let exerciseId = e.currentTarget.id
        $.ajax({
            url: alfaUrl + `WorkoutManager/GetAnExercise/${exerciseId}`,
            type: "GET"
        }).done(function (response) {
            console.log(response)
            let appHere = $(`.appendHere${exerciseId}`);
            appHere.empty;

            if (response.Exercise.Type === "Bodyweight") {
                response.SetList.forEach(function (ele) {
                    console.log(ele)
                    if (ele.id != -1) {
                        let setCont = $("<div>").addClass("row detailBox");
                        let set = $("<div>").addClass("col-4 set");
                        let reps = $("<div>").addClass("col-8 reps");
                        set.text("Set " + ele.Order + ":");
                        reps.text(ele.Reps + " reps");
               
                        appHere.append(setCont);
                        setCont.append(set);
                        setCont.append(reps);

                    }
                    else {
                        let set = $("<div>");
                        set.text("This exercise has no sets");
                        appHere.append(set);
                    }
                })

            }
            else if (response.Exercise.Type === "Strength") {
                response.SetList.forEach(function (ele) {
                    if (ele.Id != -1) {
                        let setCont = $("<div>").addClass("row detailBox");
                        let set = $("<div>").addClass("col-4 set");
                        let reps = $("<div>").addClass("col-8 reps");
                        let weight = $("<div>").addClass("weight");
                        set.text("Set " + ele.Order + ":");
                        reps.text("Reps :  " + ele.Reps);
                        weight.text("Weight :  " + ele.Weight + " lbs");

                        appHere.append(setCont);
                        setCont.append(set);
                        setCont.append(reps);
                        reps.append(weight);
                    }
                    else {
                        let set = $("<div>");
                        set.text("This exercise has no sets");
                        appHere.append(set);
                    }

                })
            }
            else if (response.Exercise.Type === "Endurance") {
                response.SetList.forEach(function (ele) {
                    if (ele.Id != -1) {
                        let goalCont = $("<div>").addClass("row detailBox");
                        let goal = $("<div>").addClass("col-4 set");
                        let dist = $("<div>").addClass("col-8 dist");
                        let time = $("<div>").addClass("time");
                        goal.text("Goal: ");
                        dist.text("Miles :  " + ele.Distance);
                        time.text("Time :  " + ele.Time + " minutes");

                        appHere.append(goalCont);
                        goalCont.append(goal);
                        goalCont.append(dist);
                        dist.append(time);
                    }
                    else {
                        let info = $("<div>");
                        info.text("No exercise goals added to this exercise");
                        appHere.append(info);
                    }

                })
            }
        })
    })
    selectExercEvents();
    function ExerciseByType (data) {
        
        let selection = data.target.id;
        $.ajax({
            url: alfaUrl + "WorkoutManager/GetExercisesByType",
            type: "GET",
            dataType: "json",
            data: {
                type: selection
            }
        }).done(function (data) {
            for (let i = 0; i < data.length; i++) {
                let exercise = $("<button>").addClass("dropdown-item");
                exercise.attr("type", "submit")
                exercise.attr("name", "name")
                exercise.attr("value", data[i])
                exercise.text(data[i]);
                $("#" + selection).siblings(".dropdown-menu").append(exercise);
            }
        })
    }
    
    function selectExercEvents() {
        $("#Strength").one("click", function (event) {
            ExerciseByType(event);
        })
        $("#Endurance").one("click", function () {
            ExerciseByType(event);
        })
        $("#Bodyweight").one("click", function () {
            ExerciseByType(event);
        })
    }

});