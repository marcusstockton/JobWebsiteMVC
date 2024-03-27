$(document).ready(function () {

    $('#jobTypeId, #showExpiredJobs').change(function () {
        $('#filterSearch').click();
    });


    $('#jobsTable tr').click(function () {
        var id = $(this).attr('id');
        console.log("Row Id clicked: " + id);

        var url = window.location.href = '/Jobs/details/' + id; //relative to domain
        $(location).attr('href', url);
    });


    // Keep the search value between clicks:
    var currentPageURL = window.location.search;
    var readParam = "currentFilter";

    var urlParams = new URLSearchParams(currentPageURL);

    // Check parameter exists and read value
    var paramValue = "";
    if (urlParams.has(readParam)) {
        paramValue = urlParams.get(readParam);
    }
    if (paramValue) {
        $('#SearchString').val(paramValue);
    }
});