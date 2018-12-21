$(document).ready(function () {
    setLength();
    getFirst();
    $("#next").on("click", function (e) {
        order++;
        getFirst();
    })
    $("#prev").on("click", function (e) {
        if (order > 0) {
            if (order + 1 >= workoutObject.length) {
                $("#greenBtn").empty();
                $("#greenBtn").append(next)
                next.on("click", function (e) {
                    order++;
                    getFirst();
                })
            }
            $(".progress-bar").removeClass("bg-success");
            order = order - 1;
            getFirst();
        }
        
    })
});


const url = "http://localhost:55601/GoMode/NextExercise";
const workout = $("#main").data("workout");
let order = 0;
let next = $("#next").clone();



const workoutObject = {
};

function setLength() {
    $.ajax({
        url: "http://localhost:55601/GoMode/GetWorkout",
        type: "GET",
        data: "json",
        data: {
            id: workoutObject.id
        }
    }).done(function (data) {
        workoutObject.length = data.length;
    })
}

function getFirst() {
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: {
            id: workoutObject.id,
            counter: order
        }
    }).done(function (data) {
        let progress = (((data.Order) - 1) / workoutObject.length) * 100;
        $(".progress-bar").attr("aria-valuenow", progress + '%')
        $(".progress-bar").attr("style", "width:" + progress + "%");

        //Add the exercise metadata
        $("#title").text(data.Exercise.Name)
        $("#sets").empty();
        $("#exercImage").empty();

        let image = $("<img>").addClass("exImg");
        image.attr(`src`, `${data.Exercise.Description}`);
        $("#exercImage").append(image);

        if (data.Exercise.Type === "Endurance") {
            displayEndurance(data);
        }
        else {
            displaySets(data);
        }
        if (order + 1 >= workoutObject.length) {
            let complete = $("#next").clone();
            complete.off();
            complete.text("Complete");
            $("#greenBtn").empty();
            $("#greenBtn").append(complete)
            complete.on("click", function (e) {
                $(".progress-bar").attr("aria-valuenow", '100%');
                $(".progress-bar").attr("style", "width:100%");
                $(".progress-bar").addClass("bg-success");
                $(e.target).text("Done");
                complete.on("click", function (e) {
                    location.assign("http://localhost:55601/WorkoutManager")
                })
            })
        }
    })
}

function displayEndurance(data) {
    
    let cont = $("<div>").addClass("row exercBox");
    let goal = $("<div>").addClass("col-6 set");
    let distance = $("<div>").addClass("col-6 reps");
    goal.text("Goal:");
    distance.text(`Distance: ${data.SetList[0].Distance} miles`);

    let time = $("<div>");
    time.text(`Time: ${data.SetList[0].Time} minutes`);

    $("#sets").append(cont);
    cont.append(goal);
    cont.append(distance);
    distance.append(time);
}

function displaySets(data) {
    data.SetList.forEach(function (set) {
        
        let setCont = $("<div>").addClass("row exercBox");
        let sets = $("<div>").addClass("col-6 set");
        let reps = $("<div>").addClass("col-6 reps");
        sets.text(`Set ${set.Order}:`);
        reps.text(`Reps: ${set.Reps}`);
        $("#sets").append(setCont);
        setCont.append(sets);
        setCont.append(reps);

        if (data.Exercise.Type === "Strength") {
            let weight = $("<div>");
            weight.text(`Weight: ${set.Weight} lbs`);
            reps.append(weight);
        }
        
    })
    if (data.SetList.length === 0) {
        let setCont = $("<div>").addClass("row exercBox");
        noSets = $("<div>").addClass("col-12");
        noSets.text("There are no sets in this exercise").addClass("text-center");

        $("#sets").append(setCont);
        setCont.append(noSets);
    }
}