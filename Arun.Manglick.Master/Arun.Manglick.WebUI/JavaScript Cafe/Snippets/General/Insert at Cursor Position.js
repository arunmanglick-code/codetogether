<script language="javascript">
			function InsertIntoDetailsBox()
			{				
				//debugger;
				idx=document.getElementById('drpFieldElements').selectedIndex;
				strToInsert=document.getElementById('drpFieldElements').options[idx].text;
				objDetailsTextBox=document.getElementById('txtRoomDetail');
				
				//IE Support
				if (document.selection)
				 {
					objDetailsTextBox.focus();
					sel = document.selection.createRange();
					sel.text = strToInsert;
					objDetailsTextBox.focus();
				}
				//MOZILLA/NETSCAPE support
				else if (objDetailsTextBox.selectionStart || objDetailsTextBox.selectionStart == '0') 
				{
					var startPos = objDetailsTextBox.selectionStart;
					var endPos = objDetailsTextBox.selectionEnd;
					objDetailsTextBox.value = objDetailsTextBox.value.substring(0, startPos) + strToInsert 	+ objDetailsTextBox.value.substring(endPos, objDetailsTextBox.value.length);
				} 
				else
				{
					objDetailsTextBox.value += strToInsert;
				}
			}	
		</script>