function parseDate(s) {
    var b = s.split(/\D/);
    return new Date(b[0], --b[1], b[2]);
}

if(window.localStorage.getItem("api-key") !== null) {
    $("#frostAPI").val(window.localStorage.getItem("api-key"));
}

(function() {
    $("#submitform").submit(function(event) {
        event.preventDefault();
        event.stopPropagation();
        if (document.getElementById('submitform').checkValidity() === true) {
            fetch('/api/postRawWork', {
                headers: new Headers({
                    'x-frost-api': $("#frostAPI").val(),
                }),
                method: 'post',
                body: JSON.stringify({
                    name: $("#workName").val(),
                    datePublished: new Date().toISOString(),
                    dateCreated: new Date(parseDate($("#dateCreated").val())).toISOString(),
                    author: $("#workAuthor").val(),
                    content: $("#workContent").val(),
                    tags: $("#workTags").val()
                })
            }).then(function(res) {
                $(":input").val("");
                $("#workContent").val("");

            }, function(rej) {
                alert('an error has occured: '+rej);
            });
        }   
    })
})();

