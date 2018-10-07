var options = {  
    weekday: "long", year: "numeric", month: "short",  
    day: "numeric", hour: "2-digit", minute: "2-digit"  
};

$("#onboard-message").html("Metrist allows you to view all works that you have submitted to the po.et blockchain and to protect your Medium posts or submit a new work.<br/>To get started create a Frost API key: <a href='https://frost.po.et'>here</a>.");

$("#submit").click(function() {
    $("#works").html("");
    fetch("/api/getAllWorks", {
        headers: new Headers({
            "x-frost-api": $("#frostAPI").val()
        })
    }).then(function(res) {
        res.text().then(function(json) {
            var works = JSON.parse(json).works;
            works.forEach(function(work) {
                $("#works").append('\
                <div class="col-md-auto"> \
                    <div class="card mb-4 box-shadow" style="max-width: 60vw;">\
                            <div class="card-body">\
                                <h3>'+ work.name +'</h3>\
                                <div style="max-height: 500px; overflow: auto;">\
                                <p class="card-text">' + work.content + '</p>\
                                </div>\
                                <div class="d-flex justify-content-between align-items-center">\
                                    <small class="text-muted">'+ work.author+' | ' + new Date(work.datePublished).toLocaleTimeString("en-us", options) + '</small>\
                                </div>\
                            </div>\
                        </div>\
                    </div>\
                ');
            });
            $("#onboard-message").html("Here are all the works that you have submitted to the po.et blockchain.");
            window.localStorage.setItem("api-key", $("#frostAPI").val());
        });
    });
});

if(window.localStorage.getItem("api-key") !== null) {
    $("#frostAPI").val(window.localStorage.getItem("api-key"));
    $("#submit").click();
}