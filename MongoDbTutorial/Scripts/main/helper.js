"use strict";

/**
 * do ajax calls easly
 * @param {string} url
 * @param {string} data
 * @param {boolean} async
 * @param {string} method
 * @param {boolean} cache
 * @param {string} dataType
 * @param {string} contentType
 * @param {string} callback
 * usage example -> ajaxCall("localhost/dropdown/getproduct", JSON.stringify({ id: id }), false, "post", true, "json", "application/json;charset=utf-8", "callbackfunctionName")
 */
function ajaxCall(url, data, async, method, cache, dataType, contentType, callbackFuncName) {
    $.ajax({
        async: async,
        method: method,
        url: url,
        cache: cache,
        data: data,
        datatype: dataType,
        contentType: contentType,
        success: function (result) {
            window[callbackFuncName](result);
            showPageLoading(false);
        },
        error: function (xhr, status, error) {
            console.log("ajax call error -> " + error);
            showPageLoading(false);
        }
    });
}

function showPageLoading(status) {
    var loadingPanel = "<div id='pageLoading' style='position:absolute;top:0;left:0;bottom:0;right:0;background-color:#fff;opacity:0.5;z-index:9999998;width:100%;height:100%;'>" +
        "<div style='position:relative;width:100%;top:50%;opacity:1;z-index:9999;'><img class='img-responsive d-block mx-auto' width='128' src='" + "/img/page_loader.gif' alt='' /></div>" +
        "</div>";

    if (status) {
        $("body").append(loadingPanel)
    } else {
        $.each($("body").find("#pageLoading"), function () {
            $(this).remove();
        });
    }
}
