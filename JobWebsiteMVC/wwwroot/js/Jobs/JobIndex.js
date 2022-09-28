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

});