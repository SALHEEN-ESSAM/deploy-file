

$(function () {

    $("#submitTBLLab1").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #labB1 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                ConClsLabProperitesid: $('#PropertyID', this).val(),
                ConClsPeriodicId: $('#drop1', this).val(),
                PeriodId: 1,
                FactoryId: 1,
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabProperty/SaveClassifcationLabProperity',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);
                        alert("تم الاضافة بنجاح");
                    }
                    else {
                        $('#Ordermessages').text(data.message);
                        alert("فشل في الحفظ");
                    }

                }

            });

        }




    });
});

$(function () {

    $("#submitTBLLab2").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #labB2 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                ConClsLabProperitesid: $('#PropertyID', this).val(),
                ConClsPeriodicId: $('#drop2', this).val(),
                PeriodId: 1,
                FactoryId: 1,
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabProperty/SaveClassifcationLabProperity',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);
                        alert("تم الاضافة بنجاح");
                    }
                    else {
                        $('#Ordermessages').text(data.message);
                        alert("فشل في الحفظ");
                    }

                }

            });

        }




    });
});

$(function () {

    $("#submitTBLLab3").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #labB3 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                ConClsLabProperitesid: $('#PropertyID', this).val(),
                IsChecked: CheckBoxValue,
                ConClsPeriodicId:0,
                PeriodId: 1,
                FactoryId: 1,
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabProperty/SaveClassifcationLabProperity',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);
                        alert("تم الاضافة بنجاح");
                    }
                    else {
                        $('#Ordermessages').text(data.message);
                        alert("فشل في الحفظ");
                    }

                }

            });

        }




    });
});

$(function () {

    $("#submitTBLLab5").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #labB5 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                ConClsLabProperitesid: $('#PropertyID', this).val(),
                ConClsPeriodicId: $('#drop1', this).val(),
                PeriodId: 1,
                FactoryId: 1,
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabProperty/SaveClassifcationLabProperity',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);
                        alert("تم الاضافة بنجاح");
                    }
                    else {
                        $('#Ordermessages').text(data.message);
                        alert("فشل في الحفظ");
                    }

                }

            });

        }




    });
});
$(function () {

    $("#submitTBLLab7").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #labB7 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                ConClsLabProperitesid: $('#PropertyID', this).val(),
                IsChecked: CheckBoxValue,
                ConClsPeriodicId: 0,
                PeriodId: 1,
                FactoryId: 1,
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabProperty/SaveClassifcationLabProperity',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);
                        alert("تم الاضافة بنجاح");
                    }
                    else {
                        $('#Ordermessages').text(data.message);
                        alert("فشل في الحفظ");
                    }

                }

            });

        }




    });
});
$(function () {

    $("#submitTBLLab8").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #labB8 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                ConClsLabProperitesid: $('#PropertyID', this).val(),
                IsChecked: CheckBoxValue,
                ConClsPeriodicId: 0,
                PeriodId: 1,
                FactoryId: 1,
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabProperty/SaveClassifcationLabProperity',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);
                        alert("تم الاضافة بنجاح");
                    }
                    else {
                        $('#Ordermessages').text(data.message);
                        alert("فشل في الحفظ");
                    }

                }

            });

        }




    });
});

$(function () {

    $("#submitTBLLab9").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #labB9 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                ConClsLabProperitesid: $('#PropertyID', this).val(),
                IsChecked: CheckBoxValue,
                ConClsPeriodicId: 0,
                PeriodId: 1,
                FactoryId: 1,
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabProperty/SaveClassifcationLabProperity',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);
                        alert("تم الاضافة بنجاح");
                    }
                    else {
                        $('#Ordermessages').text(data.message);
                        alert("فشل في الحفظ");
                    }

                }

            });

        }




    });
});

// DAdmixure classification
$(function () {

    $("#submitTBLLab4").click(function () {
        var isValid = true;

        var CheckBoxValue = $("#Technical:checked").val()
        if (CheckBoxValue != "true") {
            CheckBoxValue = "false";
        }



        var item =
        {
            MethodCuringWater: Method,
            IsTechniqcalSheet: CheckBoxValue,
            TrialMixesEvalutePeriodicId: $('#admixture').val(),

            ClsYear: 1,
            FactoryId: 1,
        }


        if (isValid) {
            var data = item

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabProperty/SaveLabAdmixtures',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);
                        alert("تم الاضافة بنجاح");
                    }
                    else {
                        $('#Ordermessages').text(data.message);
                        alert("فشل في الحفظ");
                    }

                }

            });

        }




    });
});

//leberatory testting 

$(function () {

    $("#submitTBLLab6").click(function ()
    {
        var isValid = true;
       
            var CheckBoxValue = $("#ProperProtection:checked").val()
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }

        var Method = $("#Method:checked").val()
        
        if (Method != "true")
        {
            Method = "false";
        }
        var curing = $("#curing:checked").val()

        if (curing != "true")
        {
            curing = "false";
        }
        var testing = $("#testing:checked").val()
      
        if (testing != "true")
        {
            testing = "false";
        }
        var rate = $("#rate:checked").val()
       
        if (rate != "true")
        {
            rate = "false";
        }
        var item =
        {
            MethodCuringWater: Method,
            MethodRatLoading: rate,
            MethodCuringTempl: curing,
            MehodTestingSurface: testing,        
            ProperProtection: CheckBoxValue,
            ClsYear: 1,
            FactoryId: 1,
        }
        

    if (isValid) {
        var data = item

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/ClassificationLabProperty/saveLabTestPractices',
                data: JSON.stringify(data),
                contentType: 'application/json',
                success: function (data) {
                    if (data.status) {
                        $('#Ordermessages').text(data.message);
                        alert("تم الاضافة بنجاح");
                    }
                    else {
                        $('#Ordermessages').text(data.message);
                        alert("فشل في الحفظ");
                    }

                }

            });

        }




    });
});





















