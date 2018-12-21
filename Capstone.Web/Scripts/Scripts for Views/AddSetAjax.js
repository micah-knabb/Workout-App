$(document).ready(function () {
    $("#addSet").validate({
        rules: {
            reps: {
                required: true,
                digits: true
            },
            weight: {
                required: true,
                digits: true
            }
        },
        messages: {
            reps: {
                required: "Number required",
                digits: "Only enter numbers"
            },
            weight: {
                required: "Number required",
                digits: "Only enter numbers"
            }
        },
    });

    $(".downLink").on("click", function (e) {
        let orderId = e.currentTarget.id
        let WE_Id = $("#main").data("id")
        if (orderId > 1) {
             $.ajax({
                 url: alfaUrl + "WorkoutManager/SetDown",
                type: "PUT",
                dataType: "json",
                data: {
                    Order: orderId,
                    WE_Id: WE_Id,
                }
            }).done(function (data) {
                location.reload();
            })  
        }
    });

    $(".upLink").on("click", function (e) {
        let orderId = e.currentTarget.id;
        let WE_Id = $("#main").data("id")
        let max = $("#main").data("exercises")
        if (orderId < max) {
            $.ajax({
                url: alfaUrl + "WorkoutManager/SetUp",
                type: "PUT",
                dataType: "json",
                data: {
                    Order: orderId,
                    WE_Id: WE_Id,
                }
            }).done(function (data) {
                location.reload();
            })
        }
    });

    $(".deleteSet").on("click", function (e) {
        let SetId = e.currentTarget.id
        let WE_Id = $("#main").data("id")
        if (confirm('Are you sure?')) {

            $.ajax({
                url: alfaUrl + "WorkoutManager/DeleteSet",
                type: "DELETE",
                dataType: "json",
                data: {
                    SetId: SetId,
                    WE_Id: WE_Id
                }
            }).done(function (data) {
                location.reload();
            })
        }
    });

    $("#addSet").submit(function (e) {
        e.preventDefault();
        let id = $("#main").data("id");
        let reps = e.target.elements.reps.value;
        let weight = e.target.elements.weight.value;

        $.ajax({
            url: alfaUrl + "WorkoutManager/AddStrengthSet",
            type: "POST",
            dataType: "json",
            data: {
                reps: reps,
                weight: weight,
                workoutExercise: id
            }
        }).done(function (data) {
            let cardBox = $("<div>").addClass("card container")
            let cardHeader = $("<div>").addClass("card-header setRow row")
            let container = $("<div>").addClass("container")
            let row = $("<div>").addClass("row")
            let orderContainer = $("<div>").addClass("col-md-3 setInfo")
            let arrowUp = $("<button>").addClass("btn btn-link fas fa-caret-up")
            let savedOrder = $("<div>").text(data.Order).addClass("setInfo")
            let arrowDown = $("<div>").addClass("btn btn-link fas fa-caret-down")
            let savedReps = $("<div>").text(data.Reps).addClass("col setInfo")
            let savedWeight = $("<div>").text(data.Weight).addClass("col setInfo")
            let deleteLink = $("<div>").text("Delete").addClass("col btn btn-link deleteSet").attr('id', data.Id)

            cardBox.append(cardHeader)
            cardHeader.append(container)
            container.append(row)
            orderContainer.append(arrowUp)
            orderContainer.append(savedOrder)
            orderContainer.append(arrowDown)
            row.append(orderContainer)
            row.append(savedReps)
            row.append(savedWeight)
            row.append(deleteLink)
            //row.append(secEmptyCol)

            $("#addSetHere").append(cardBox) 
            location.reload()
        })
    })
});