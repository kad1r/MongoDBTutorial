"use strict";

var ids = [];

$(function () {
    $("body").on("click", ".delete", function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();

        var deleteForm = $("#deleteForm"),
            deleteRecord = $("#deleteRecord"),
            url = deleteForm.length > 0 ? deleteForm.val() : deleteRecord.length > 0 ? deleteRecord.val() : "";

        showPageLoading(true);
        ids.length = 0;

        // get selected checkbox values
        $("input:checkbox").each(function () {
            var th = $(this);

            if (th.is(":checked")) {
                ids.push(th.val());
            }
        });

        if (url != "" && ids.length > 0) {
            ajaxCall(url, JSON.stringify({ ids: ids }), false, "post", true, "json", "application/json;charset=utf-8", "deletionResult");
        }
    });
});

function deletionResult(response) {
    if (response != null && !response.HasError) {
        location.reload(true);
    } else {
        alert(response.ErrorMessage);
    }
}