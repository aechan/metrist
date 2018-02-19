
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

$(document).ready(function() {
    if(getParameterByName("code")) {
        var accessCode = getParameterByName("code");
        fetch('/api/getMediumAccessToken', {
            headers: new Headers({
                'x-medium-code': accessCode
            })
        }).then(function(resp){
            resp.json().then(function(json) {
                console.log("Got: " + json);
                var config = JSON.parse(cookieHandler.get("reg"));
                config.medium_api = json;
                console.log(JSON.stringify(config));
                cookieHandler.set("reg", JSON.stringify(config));
            });
        });
        
    } else {
        $("#mediumreg").click(function() {
            cookieHandler.set("reg", JSON.stringify({
                frost_api: $("#formFrost").val(),
                medium_api: {}
            }));
            fetch('/api/getMediumAuthURL').then(function(resp){
                resp.text().then(function(data){
                    //console.log(data);
                    var win = window.open(data, "_blank");
                    win.focus();
                });
            });
        });
    }
});
