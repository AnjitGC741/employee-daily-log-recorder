
$(document).ready(function () {

    $('#employeeLoginForm').validate({
        rules: {
            email: {
                required: true,
                email: true,
            },
            password: {
                required: true,
                minlength: 5
            }
        },
        messages: {
            email: {
                required: "Email cannot be empty",
                email: "Enter valid email"
            },
            password: {
                required: "Password cannot be empty",
                minlength: "Password length must be at least 5 characters"
            }
        }
    });


    $("#showPasswordCheckBox").on("change", () => {
        if ($("#showPasswordCheckBox").prop("checked")) {
            $("#password").attr("type", "text");
        } else {
            $("#password").attr("type", "password");
        }
    });
});