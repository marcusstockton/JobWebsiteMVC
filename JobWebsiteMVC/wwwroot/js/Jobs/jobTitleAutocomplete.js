$(function () {
    $("#JobTitle").autocomplete({
        source: function (request, response) {
            $.ajax(
                {
                    url: "/api/JobTitles/" + request.term,
                    dataType: "json",
                    success: function (data) {
                        response($.map(data, function (value, key) {
                            console.log(`Returned: ${value.description}`);
                            return {
                                label: value.description,
                                value: value.description,
                                id: key
                            }
                        }));
                    }
                });
        },
        minLength: 2,
        delay: 500,
    });
});