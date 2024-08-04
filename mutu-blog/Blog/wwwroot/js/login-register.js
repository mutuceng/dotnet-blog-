$(document).ready(function() {
    // Button elements for switching between sign-up and sign-in forms
    $('#register').click(function(){
        window.location.href = 'Users/User/Register';
    });

    $('#login').click(function(){
        window.location.href = 'Users/User/Login';
    });
});
