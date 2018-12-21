///<reference path="../jquery-3.1.1.js" />
///<reference path="../jquery.validate.js" />

$(document).ready(function () {
    $("#Registration").validate({
        rules: {
            FirstName: {
                required: true
            },

            LastName: {
                required: true
            },

            Username: {
                required: true,
                minlength: 5,
            },

            Email: {
                required: true,
                email: true,

            },
            Password: {
                required: true,         
                minlength: 5,           
            },
            ConfirmPassword: {
                equalTo: "#Password"
            },

        },
        messages: {
            FirstName: {
                required: "First name required"
            },
            LastName: {
                required: "Last name required"
            },
            Username: {
                required: "Username required and must be at least 5 characters"
            },
            Email: {
                required: "Email is required",
                email: "Valid email is required",
            },
            Password: {
                required: "Password required"
            },
            ConfirmPassword: {
                equalTo: "Passwords must match",
            },
        },
        submitHandler: function (form) {
            form.submit();
        }
    })
})