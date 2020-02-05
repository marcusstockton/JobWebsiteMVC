const BASE_URL = "http://api.dataatwork.org/v1/";

function jobTitleAutoComplete(jobTitle) {
    // curl -X GET /path/to/api/v1/jobs/autocomplete?contains="software"
    if (jobTitle.length > 3) {
        $.get(`BASE_URL/jobs/autocomplete?contains="${jobTitle}"`)
            .done(function (result) {
                return result;
            })
            .fail(function (error) {

            });
    }
}

function skillNameAutoComplete(skillName) {
    // curl -X GET /path/to/api/v1/skills/autocomplete?contains="java"
    if (skillName.length > 3) {
        $.get(`BASE_URL/skills/autocomplete?contains="${skillName}"`)
            .done(function (result) {

            })
            .fail(function (error) {

            });
    }
}
