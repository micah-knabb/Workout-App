///<reference path="../jquery-3.1.1.js" />
///<reference path="../jquery.validate.js" />

$(document).ready(function () {
    $("#LoginForm").validate({
        rules: {
            Username: {
                required: true
            },
            Password: {
                required: true
            }
        },
        messages: {
            Username: {
                required: "Username required"
            },
            Password: {
                required: "Password required"
            }
        },
        submitHandler: function (form) {
            form.submit();
        }
    })
})