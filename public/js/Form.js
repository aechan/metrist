if(window.localStorage.getItem("api-key") !== null) {
    $("#frostAPI").val(window.localStorage.getItem("api-key"));
}
(function() {
    $("#submitform").submit(function(event) {
        event.preventDefault();
        event.stopPropagation();
        if (document.getElementById('submitform').checkValidity() === true) {
            fetch('/api/postWork', {
                headers: new Headers({
                    'x-frost-api': $("#frostAPI").val(),
                    'x-medium-api': $("#mediumName").val()
                }),
                method: 'post',
                body: $("#mediumURL").val()
            }).then(function(res) {
                $("#mediumName").val("");
                $("#frostAPI").val("");
                $("#mediumURL").val("");
            }, function(rej) {
                alert('an error has occured: '+rej);
            });
        }   
    })
})();