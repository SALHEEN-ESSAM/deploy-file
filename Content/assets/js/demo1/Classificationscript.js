 // GET: Admin/Classification
 //ادخال التجهيزات والمتطلبات الاساسية للمصنع
$(function () {
   
    $("#submitCollabs").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #Collabs tr").each(function () {
          
            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue!="true")
            {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }
          
            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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



  // GET: Admin/ClassificationFactory
  //ادخال التجهيزات والمتطلبات التكميلية للمصنع

$(function () {

    $("#submitTBL1").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #Tbl1 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
           
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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

    $("#submitTBL2").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #Tbl2 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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

    $("#submitTBL3").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #Tbl3 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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

    $("#submitTBL5").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #Tbl5 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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

    $("#submitTBL6").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #Tbl6 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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

    $("#submitTBL7").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #Tbl7 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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
//----------------------//
 // ادخال التجهيزات والمتطلبات الاساسية للمختبر
// GET: Admin/ClassificationLab

$(function () {

    $("#submitTBLLab1").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #TBLLab1 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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
        $("#Classfication #TBLLab2 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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
//---------------------//

//ادخال التجهيزات والمتطلبات التكميلية للمختبر
// GET: Admin/ClassificationLabAddtion

$(function () {

    $("#LAB1").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #LABAdd1 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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

    $("#LAB2").click(function () {
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #LABAdd2 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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

    $("#LAB3").click(function () {
        
        //debugger;
        var isValid = true;
        var itemsList = [];
        $("#Classfication #LABAdd3 tr").each(function () {

            var CheckBoxValue = $("#PropertyValue:checked", this).val();
            if (CheckBoxValue != "true") {
                CheckBoxValue = "false";
            }
            var item = {
                FactoryId: $('#FactoryId', this).val(),
                ConClsPropertyId: $('#PropertyID', this).val(),
                PropertyValue: CheckBoxValue,
                ClsYear: $('#ClsYear', this).val(),
            }

            itemsList.push(item);
        });

        if (isValid) {
            var data = itemsList

            //$("#submit").attr("disabled", true);

            $.ajax({
                type: 'POST',
                url: '/ar/Admin/Classification/SaveClassifcation',
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







