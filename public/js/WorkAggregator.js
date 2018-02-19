var options = {  
    weekday: "long", year: "numeric", month: "short",  
    day: "numeric", hour: "2-digit", minute: "2-digit"  
};  

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
        });
    });
});