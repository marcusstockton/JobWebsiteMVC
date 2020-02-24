$(document).ready(function(){
    // Do Stuff
    $("#JobTitle").autocomplete({
        source: function (request, response) {
            $.ajax(
                {
                    url: "/Job/GetJobTitleAutoComplete",
                    dataType: "json",
                    data:
                    {
                        jobtitle: request.term,
                    },
                    success: function (data) {
                        response($.map(data, function (value, key) {
                            return {
                                label: value,
                                value: value,
                                id: key
                            }
                        }));
                    }
                });
        },
        minLength: 2,
        delay: 500,
        select: function (event, ui) {
            // var jobId = ui.item ? ui.item.id : 0;
            // console.log("JobId: ", jobId);
            // if (jobId !== '') {
            //     // Go Get skills for the selected job???
            //     let url = "/Job/GetJobSkillAutoComplete?jobId=" + jobId;
            //     $.get(url)
            //         .done((response)=>{
            //             console.log(response);
            //         })
            //         .catch((err)=>{
            //             console.log(err);
            //         });
            // }
        }
    });
    
});