//Project: Case Study2
//Purpose: Demonstrate Crud functionality and REST understanding
//Coder: Sonia Friesen, 0813682
//Date: Due oct 23rd.

$(function () {
        
    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirstname: { maxlength: 25, required: true },
            TextBoxLastname: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true }
        },
        errorElement: 'div',
        messages: {
            TextBoxTitle: {
                required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            TextBoxFirstname: {
                requried: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxLastname: {
                required: "required 1- 25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxPhone: {
                required: "required 1-15 chars.", maxlength: "required 1-15 chars."
            },
            TextBoxEmail: {
                required: "required 1- 40 chars.", maxlength: "required 1- 40 chars.", email: "need valid email formate"
            }
        }
    });// EmployeeModalForm.validate

    $.validator.addMethod("validTitle", (value) => { //custom rule
        return (value === "Mr." || value === "Ms." || value === "Mrs." || value === "Dr.");
    }, ""); //.validator.addMethod

    //gets all the employee information 
    const getAll = async (msg) => {
        try {
            $("#employeeList").text("Finding Employee Information...");
            let response = await fetch(`api/employee`);
            if (!response.ok) // or check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json();
            buildEmployeeList(data, true);
            msg === "" ? //are we appending to an existing message
                $("#status").text("Employees Loaded") : $("#status").text(`${msg} - Employees Loaded`);
            response = await fetch(`api/department`);
            if (!response.ok) // or check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let divs = await response.json();
            sessionStorage.setItem('alldepartments', JSON.stringify(divs));
        } catch (error) {
            $("#status").text(error.message);
        }
    };//get all

    //using the <select> does a dropdown option menu
    const loadDivisionDDL = (studiv) => {
        html = '';
        $('#ddlDeps').empty();
        let alldepartment = JSON.parse(sessionStorage.getItem('alldepartments'));
        alldepartment.map(div => html += `<option value="${div.id}">${div.name}</option>`);
        $('#ddlDeps').append(html);
        $('#ddlDeps').val(studiv);
    };//loadDivisionDDL

    //set intial values for an update
    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update Employee</h4>");
        $("#deletebutton").show();
        clearModalFields();
        data.map(employee => {
            if (employee.id === parseInt(id)) {
                $("#TextBoxTitle").val(employee.title);
                $("#TextBoxFirstname").val(employee.firstname);
                $("#TextBoxLastname").val(employee.lastname);
                $("#TextBoxPhone").val(employee.phoneno);
                $("#TextBoxEmail").val(employee.email);
                $('#ImageHolder').html(`<img height="120" width="110" src="data:image/png;base64,${employee.staffPicture64}"/>`);
                sessionStorage.setItem("Id", employee.id);
                sessionStorage.setItem("DepartmentId", employee.departmentId);
                sessionStorage.setItem("Timer", employee.timer);
                sessionStorage.setItem("Picture", employee.staffPicture64);
                $("#modalstatus").text("update data");
                loadDivisionDDL(employee.departmentId.toString());
                $("#theModal").modal("toggle");
            }//end if            
        });
    }; //setupUpdate

    //sets the intial values for an add
    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#deletebutton").hide();
        $("#modaltitle").html("<h4>add Employee</h4>");
        $("#theModal").modal("toggle");
        $("#modalStatus").text("add new Employee");
        loadDivisionDDL(-1);
        clearModalFields();

    };//setupFroadd

    //clears all text fislds
    const clearModalFields = () => {
        let validator = $("#EmployeeModalForm").validate();
        validator.resetForm();

        $("#TextBoxTitle").val("");
        $("#TextBoxFirstname").val("");
        $("#TextBoxLastname").val("");
        $("#TextBoxPhone").val("");
        $("#TextBoxEmail").val("");        
        sessionStorage.removeItem("Id");
        sessionStorage.removeItem("DepartmentId");
        sessionStorage.removeItem("Timer");
       
    };// clear modal fields

    //gives us that table look bu inserting the div into the main body of our employee html
    const buildEmployeeList = (data, usealldata) => {
        $("#employeeList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id ="status">EmployeeInfo</div>
                 <div class="list-group-item row d-flex text-center"  id="heading">
                 <div class="col-4 h4 ">Title</div>
                 <div class="col-4 h4 ">First</div>
                 <div class="col-4 h4">Last</div>
                 </div>`);
        div.appendTo($("#employeeList"));
        usealldata ? sessionStorage.setItem("allEmployees", JSON.stringify(data)) : null;
        btn = $(`<button class ="list-group-item row d-flex" id="0"><div class= "col-12 text-center">...click to add Employee</div></button>`);  //creating the add button 
        btn.appendTo($("#employeeList"));
        data.map(emp => {
            btn = $(`<button class="list-group-item row d-flex" id="${emp.id}">`);          //getting employee values to show in the employee html and making them all buttons for an update.
            btn.html(`<div class="col-4" id="employeetitles${emp.id}">${emp.title}</div>
                      <div class="col-4" id="employeefnames${emp.id}">${emp.firstname}</div>
                      <div class="col-4" id="employeelastname${emp.id}">${emp.lastname}</div>`
            );
            btn.appendTo($("#employeeList"));
        });
    };// buildStudentList

    //update function that changes any employee information.
    const update = async () => {
        try {
            emp = new Object();
            emp.Title = $("#TextBoxTitle").val();
            emp.Firstname = $("#TextBoxFirstname").val();
            emp.Lastname = $("#TextBoxLastname").val();
            emp.Phoneno = $("#TextBoxPhone").val();
            emp.Email = $("#TextBoxEmail").val();
            emp.DepartmentId = $('#ddlDeps').val();
            //we store these 3 earlier
            emp.Id = sessionStorage.getItem("Id");
           //emp.DepartmentId = sessionStorage.getItem("DepartmentId");
            emp.Timer = sessionStorage.getItem("Timer");
            emp.staffPicture64 = sessionStorage.getItem("Picture")
                ? emp.staffPicture64 = sessionStorage.getItem("Picture")
                : null;
            //sent the updated back to the server asychronously using PUT
            let response = await fetch("/api/employee", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json; charset+utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok)
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $("#status").text(`${response.status}, Text - ${response.statusText}`);
            }
            $("#theModal").modal("toggle");
        } catch (error) {
            $("#status").text(error.message);
        }
    }; //update

    //add function to add an employee
    const add = async () => {
        try {
            emp = new Object();
            emp.Title = $("#TextBoxTitle").val();
            emp.Firstname = $("#TextBoxFirstname").val();
            emp.Lastname = $("#TextBoxLastname").val();
            emp.Phoneno = $("#TextBoxPhone").val();
            emp.Email = $("#TextBoxEmail").val();
            emp.DepartmentId = $('#ddlDeps').val();
            emp.Id = -1;
            emp.Timer = null;
            emp.staffPicture64 = null;
            //send the student info to the server asynchronously using POST
            let response = await fetch("/api/employee", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok)
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $("#status").text(`${response.status}, Text - ${response.statusText}`);
            }
            $("#theModal").modal("toggle");
        } catch (error)
        {
            $("#status").text(error.message);
        }
    };//add

    //a delete function to delete any employee from clicking on them
    const _delete = async () => {
        try {
            let response = await fetch(`api/employee/${sessionStorage.getItem('Id')}`, {
                method: 'DELETE',
                headers: { 'Content-Type' : 'application/json; charset=utf-8'}
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            }else {
            $("#status").text(`Status - ${response.status}, Problem on delete server side`);
            }
            $("#theModal").modal("toggle");
        } catch (error)
        {
        $("#status").text(error.message);
        }        
    };

    //determines if we are clicking on an update or add 
    $("#actionbutton").click(() => {
        $("#modalstatus").removeClass(); //remoce any existing css on div
        if ($("#EmployeeModalForm").valid()) {            
            $("#modalstatus").attr("class", "badge badge-success");
            $("#modalstatus").text("data entered is valid");
        }
        else {
            $("#modalstatus").attr("class", "badge badge-danger");
            $("#modalstatus").text("fix errors");
            e.preventDefault;
            return;
        }

        $("#actionbutton").val() === "update" ? update() : add();
    });//actionbutton click

    //if we delete , we want to confirm that this is what we want to do
    $('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle = confirmation]' });
    $('#deletebutton').click(() => _delete()); //yes if was choosen

    $("#srch").keyup(() => {
        let alldata = JSON.parse(sessionStorage.getItem("allEmployees"));
        let filtereddata = alldata.filter((stu) => stu.lastname.match(new RegExp($("#srch").val(), 'i')));
        buildEmployeeList(filtereddata, false);
    }); //search keyup

    //click event for when we want to update/delete an employee by clicking on their name or space in the table.
    $("#employeeList").click((e) => {       
        if (!e) e = window.event;
        let Id = e.target.parentNode.id;
        if (Id === "employeeList" || Id === "") {
            Id = e.target.id;
        }

        if (Id !== "status" && Id !== "handling") {
            let data = JSON.parse(sessionStorage.getItem("allEmployees"));
            Id === "0" ? setupForAdd() : setupForUpdate(Id, data);               
        } else {
            return false; // ignore if they clicked on heading or status
        }
    });//student click    

    //do we have a picture
    $("input:file").change(() => {
        const reader = new FileReader();
        const file = $("#uploader")[0].files[0];

        file ? reader.readAsBinaryString(file) : null;

        reader.onload = (readerEvt) => {
            //get binary data then convert to encoded string
            const binaryString = reader.result;
            const encodedString = btoa(binaryString);
            $('#ImageHolder').html(`<img height="120" width="110" src="data:image/png;base64,${encodedString}"/>`);


            sessionStorage.setItem('Picture', encodedString);
        };
    });//input file

    getAll(""); //first grab the data fom the server
});