function fnCheck() {
	return document.getElementById("<%=txtCheckin%>").value.trim(), "" == $("#txtCheckin").val() ? ($("#txtCheckin").css("border-color", "red"), $("#txtCheckin").focus(), !1) : ($("#txtCheckin").css("border-color", ""), "" == $("#txtCheckout").val() ? ($("#txtCheckout").css("border-color", "red"), $("#txtCheckout").focus(), !1) : void $("#txtCheckout").css("border-color", ""))
}

function changeStartDate() {
	$("#txtCheckin").datepicker({
		dateFormat: "yy-mm-dd",
		onSelect: showDays,
		minDate: 0,
		numberOfMonths: 1,
		buttonImageOnly: !0
	})
}

function changeEndDate() {
	$("#txtCheckout").datepicker({
		minDate: 0,
		onSelect: showMoreDays,
		dateFormat: "yy-mm-dd",
		numberOfMonths: 1,
		buttonImageOnly: !0
	})
}

function showMoreDays() {
	var t = $("#txtCheckin").datepicker("getDate"),
		e = $("#txtCheckout").datepicker("getDate");
	t < e ? t && e && (t = (e - t) / 1e3 / 60 / 60 / 24, t = Math.round(t), $("#txtNights").val(t)) : ($("#txtCheckout").val(" "), $("#txtNights").val(" "))
}

function showDays() {
	var t = $("#txtCheckin").datepicker("getDate");
	t.setDate(t.getDate() + 1), $("#txtCheckout").datepicker("setDate", t);
	t = ($("#txtCheckout").datepicker("getDate") - t) / 1e3 / 60 / 60 / 24, t = Math.round(t);
	$("#txtNights").val(t + 1)
}
window.addEventListener("load", function (t) {
	$("img.lazyload").lazyload()
}), $(document).ready(function () {
	changeStartDate(), changeEndDate()
});