$(document).ready(function() {
    var config = JSON.parse(cookieHandler.get("reg"));

    fetch("/api/getAllWorks", {
        headers: new Headers({
            "x-frost-api": config.frost_api
        })
    }).then(function(res) {
        res.text().then(function(json) {
            console.log(json);
            var works = JSON.parse(json).works;
            works.forEach(function(work) {
                $("#works").append('\
                <div class="col-md-4"> \
                    <div class="card mb-4 box-shadow">\
                            <div class="card-body">\
                                <h3>'+ work.name +'</h3>\
                                <p class="card-text">' + work.content + '</p>\
                                <div class="d-flex justify-content-between align-items-center">\
                                    <div class="btn-group">\
                                    <button type="button" class="btn btn-sm btn-outline-secondary">View</button>\
                                    <button type="button" class="btn btn-sm btn-outline-secondary">Edit</button>\
                                    </div>\
                                    <small class="text-muted">9 mins</small>\
                                </div>\
                            </div>\
                        </div>\
                    </div>\
                ');
            });
        });
    });
});