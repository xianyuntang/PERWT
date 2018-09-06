var globalTimeout = null;
$(document).ready(function () {
    $("#Subject").keyup(function () {
        var partitialTitle = $("#Subject").val();
        if (globalTimeout != null) {
            clearTimeout(globalTimeout);
        }
        globalTimeout = setTimeout(function () {
            globalTimeout = null;
            $.ajax(
                {
                    type: "GET",
                    url: "/GetRecentReport",
                    data: {
                        partitialTitle: partitialTitle
                    },
                    dataType: "json",

                    success: function (response) {
                        document.getElementById("reportcontainer").innerHTML = "";


                        $.each(response,
                            function (i, item) {
                                var tr;
                                if (String(item.Result) === "確認為釣魚郵件") {
                                    tr = document.createElement("tr");
                                    tr.className = "danger";
                                    var td1 = document.createElement("td");
                                    var node = document.createTextNode(item.Subject);
                                    td1.appendChild(node);
                                    tr.appendChild(td1);
                                    var td2 = document.createElement("td");
                                    var node = document.createTextNode(item.Time);
                                    td2.appendChild(node);
                                    tr.appendChild(td2);
                                    var td3 = document.createElement("td");
                                    var node = document.createTextNode(item.Result);
                                    td3.appendChild(node);
                                    tr.appendChild(td3);
                                } else {
                                    tr = document.createElement("tr");
                                    var td1 = document.createElement("td");
                                    var node = document.createTextNode(item.Subject);
                                    td1.appendChild(node);
                                    tr.appendChild(td1);
                                    var td2 = document.createElement("td");
                                    var node = document.createTextNode(item.Time);
                                    td2.appendChild(node);
                                    tr.appendChild(td2);
                                    var td3 = document.createElement("td");
                                    var node = document.createTextNode(item.Result);
                                    td3.appendChild(node);
                                    tr.appendChild(td3);
                                }


                                var element = document.getElementById("reportcontainer");
                                element.appendChild(tr);


                            });
                    },
                    error: function () { alert("伺服器異常請稍後再試") }


                });

        },
            500);
    });

});

function checkDuplicate() {

    var checkValue = document.getElementById("reportcontainer").innerHTML;
    if (checkValue !== "") {
        var r = confirm("已有相關通報，確定要通報嗎？");
        if (r === true) {
            return true;
        } else {
            return false;
        }

    } else {
        return true;
    }
};