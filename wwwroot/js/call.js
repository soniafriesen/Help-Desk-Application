//Project: Case Study2
//Purpose: Demonstrate Crud functionality and REST understanding, with additional custom functions for Calls added
//Coder: Sonia Friesen, 0813682
//Date: Due Dec 11 2019
$(function () {

//use a validator to validate the drop down lists and notes
    $("#CallModalForm").validate({
        rules: {
            ddlProbs: {required: true},
            ddlEmps: {required: true },
            ddlTechs: {required: true },
            TextBoxNotes: { maxlength: 250, required: true}           
        },
        errorElement: 'div',
        messages: {
            ddlProbs: {
                required: "Problem is required"
            },
            ddlEmps: {
                requried: "Employee is required"
            },
            ddlTechs: {
                required: "Technician is required"
            },
            TextBoxNotes: {
                required: "required 1-250 chars.", maxlength: "required 1-250 chars."
            }            
        }
    });// CallModalForm.validate

    //getall function
    const getAll = async (msg) => {
        try {
            $("#callList").text("Finding Call Information...");
            let response = await fetch(`api/call`);
            if (!response.ok) // or check for response.status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json();
            buildCallList(data, true);
            msg === "" ? //are we appending to an existing message
                $("#status").text("Calls Loaded") : $("#status").text(`${msg} - Employees Loaded`);           
        } catch (error) {
            $("#status").text(error.message);
        }
    };//get all

    //fetch apis and loadhtme in the drop down list
    let loadEmployeeDDL = async () => {
        response = await fetch(`api/employee`);
        if (!response.ok)
            throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
        let emp = await response.json();     
       
        html = '';        
        $('#ddlEmps').empty();       
        emp.map(div => html += `<option value="${div.id}">${div.lastname}</option>`);
        $('#ddlEmps').append(html);  
        $('#ddlEmps').val("");

        
        $('#ddlTechs').empty();
        html = ''; 
        const tech = emp.filter( techEmp => techEmp.isTech === true);
        tech.map(div => html += `<option value="${div.id}">${div.lastname}</option>`);
        $('#ddlTechs').append(html);  
        $('#ddlTechs').val("");
    };//loadEmployeeDDL

    let loadProblemDDL = async (callid) => {
    response = await fetch(`api/problem`);
    if (!response.ok)
        throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
    let cours = await response.json();

    html = '';
    $('#ddlProbs').empty();
    cours.map(div => html += `<option value="${div.id}">${div.description}</option>`);
        $('#ddlProbs').append(html);
        $('#ddlProbs').val(callid);

    };//loadProblemDDL
    //foramted date
    const formatDate = (date) => {
        let d;
        date === (undefined) ? d = new Date() : d = new Date(Date.parse(date));
        let _day = d.getDate();
        let _month = d.getMonth() + 1;
        let _year = d.getFullYear();
        let _hour = d.getHours();
        let _min = d.getMinutes();
        if (_min < 10) { _min = "0" + _min; }

        if (_year > 2030) return "";

        return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
    };//formateDate()

    //clear model fields   
    const clearModalFields = () => {
        let validator = $("#CallModalForm").validate();
        validator.resetForm();

        $("#ddlProbs").val("");
        $("#ddlEmps").val("");
        $("#ddlTechs").val("");
        $("#TextBoxNotes").val("");        
        sessionStorage.removeItem("Id");
        sessionStorage.removeItem("ProblemId");
        sessionStorage.removeItem("Timer");
        sessionStorage.removeItem("EmployeeId");     
    };// clear modal fields


    //setupforupdate
    //set intial values for an update
    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update Call</h4>");
        $("#deletebutton").show();
        clearModalFields();
        data.map(call => {
            if (call.id === parseInt(id)) {
                loadEmployeeDDL();
                loadProblemDDL();
                $("#labelDateOpened").text(formatDate(call.dateOpened));
                $("#labelDateOpened").attr('readonly', true);                
                $("#checkboxCall").show();
                if (!call.openStatus) {
                    $("#ddlEmps").attr(disabled, true);
                    $("#ddlTechs").attr(disabled, true);
                    $("#ddlProbs").attr(disabled, true);
                    $("#labelDateClosed").text(formatDate(call.dateClosed));
                    ("#labelDateClosed").attr('readonly', true);
                }
                else {
                    $("#ddlEmps").attr(disabled, false);
                    $("#ddlTechs").attr(disabled, false);
                    $("#ddlProbs").attr(disabled, false);
                }
                sessionStorage.setItem("Id", call.id);
                sessionStorage.setItem("EmployeeId", call.employeeId);
                sessionStorage.setItem("ProblemId", call.problemId);
                sessionStorage.setItem("TechId", call.techId);
                sessionStorage.setItem("dateOpened", formateDate(call.dateOpened));
                sessionStorage.setItem("dateClosed", formateDate(call.dateClosed));
                sessionStorage.setItem("Timer", call.timer);                  
                $("#modalstatus").text("update data");               
                $("#theModal").modal("toggle");
            }//end if            
        });
    }; //setupUpdate
    const update = async () => {
        try {
            call = new Object();
            call.employeeId = $("#ddlEmps").val();
            call.problemId = $("#ddlProbs").val();
            call.techId = $("#ddlTechs").val();
            call.dateOpened = sessionStorage.getItem("dateOpened");
            call.dateClosed = sessionStorage.getItem("dateClosed");

            //we store these 3 earlier
            call.Id = sessionStorage.getItem("Id");
            //emp.DepartmentId = sessionStorage.getItem("DepartmentId");
            call.Timer = sessionStorage.getItem("Timer");
            
            //sent the updated back to the server asychronously using PUT
            let response = await fetch("/api/call", {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json; charset+utf-8"
                },
                body: JSON.stringify(emp)
            });
            if (response.ok) {
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

    //sets the intial values for an add
    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#deletebutton").hide();
        $("#modaltitle").html("<h4>add Calls</h4>");
        $("#theModal").modal("toggle");
        $("#modalStatus").text("add new Call");
        loadEmployeeDDL();
        loadProblemDDL();
        $("#TextBoxNotes").val("");
        $("#labelDateOpened").html(formateDate());
        $("#dateClosedRow").hide();
        $("#closeCallRow").hide();
        clearModalFields();
    };//setupFroadd

    //add function to add an employee
    const add = async () => {
        try {
            call = new Object();
            call.Id = -1;
            call.employeeId = $('#ddlEmps').val();
            call.problemId = $("#ddlProbs").val();
            call.problemDescription = $("#ddlProbs").val();
            call.techId = $("#ddlTechs").val();
            call.employeeName = $('#ddlEmps').val();
            call.techName = $("#ddlTechs").val();           
            call.Timer = null;
            call.dateClosed = null;
            call.dateOpened = formateDate();
            call.openStatus = true;
            call.notes = $("#TextBoxNotes").val();
            //send the student info to the server asynchronously using POST
            let response = await fetch("/api/call", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(call)
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $("#status").text(`${response.status}, Text - ${response.statusText}`);
            }
            $("#theModal").modal("toggle");
        } catch (error) {
            $("#status").text(error.message);
        }
    };//add

    //a delete function to delete any employee from clicking on them
    const _delete = async () => {
        try {
            let response = await fetch(`api/call/${sessionStorage.getItem('Id')}`, {
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $("#status").text(`Status - ${response.status}, Problem on delete server side`);
            }
            $("#theModal").modal("toggle");
        } catch (error) {
            $("#status").text(error.message);
        }
    };

    //build the main body of calls and the list of calls    
    const buildCallList = (data, usealldata) => {
        $("#callList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id ="status">EmployeeInfo</div>
                 <div class="list-group-item row d-flex text-center"  id="heading">
                 <div class="col-4 h4 ">Date</div>
                 <div class="col-4 h4 ">For</div>
                 <div class="col-4 h4">Problem</div>
                 </div>`);
        div.appendTo($("#callList"));
        usealldata ? sessionStorage.setItem("allCalls", JSON.stringify(data)) : null;
        btn = $(`<button class ="list-group-item row d-flex" id="0"><div class= "col-12 text-center">...click to add a Call</div></button>`);  //creating the add button 
        btn.appendTo($("#callList"));
        data.map(call => {
            btn = $(`<button class="list-group-item row d-flex" id="${call.id}">`);          //getting employee values to show in the employee html and making them all buttons for an update.
            btn.html(`<div class="col-4" id="calldates${call.id}">${call.dateOpened}</div>
                      <div class="col-4" id="callnames${call.id}">${call.employeeName}</div>
                      <div class="col-4" id="callproblem${call.id}">${call.problemDescription}</div>`
            );
            btn.appendTo($("#callList"));
        });
    };// buildCallList

    $("#actionbutton").click(() => {
        $("#modalstatus").removeClass(); //remoce any existing css on div
        if ($("#CallModalForm").valid()) {
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
    });//actionbutton click'
    //if we delete , we want to confirm that this is what we want to do
    $('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle = confirmation]' });
    $('#deletebutton').click(() => _delete()); //yes if was choosen

    $("#srch").keyup(() => {
        let alldata = JSON.parse(sessionStorage.getItem("allCalls"));
        let filtereddata = alldata.filter((stu) => stu.employeeName.match(new RegExp($("#srch").val(), 'i')));
        buildCallList(filtereddata, false);
    }); //search keyup
    //are we clicking on the list to update or add
    //click event for when we want to update/delete an employee by clicking on their name or space in the table.
    $("#callList").click((e) => {
        if (!e) e = window.event;
        let Id = e.target.parentNode.id;
        if (Id === "callList" || Id === "") {
            Id = e.target.id;
        }

        if (Id !== "status" && Id !== "handling") {
            let data = JSON.parse(sessionStorage.getItem("allCalls"));
            Id === "0" ? setupForAdd() : setupForUpdate(Id, data);
        } else {
            return false; // ignore if they clicked on heading or status
        }
    });//call click  
    getAll("");
});