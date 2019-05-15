<?xml version='1.0' encoding='ASCII' ?>
<Velocity11 file='Protocol_Data' md5sum='cfdeeb1825abda846374881561a53eb2' version='2.0' >
	<File_Info AllowSimultaneousRun='1' AutoExportGanttChart='0' AutoLoadRacks='When the main protocol starts' AutoUnloadRacks='0' AutomaticallyLoadFormFile='1' Barcodes_Directory='' ClearInventory='0' DeleteHitpickFiles='1' Description='' Device_File='C:\Users\tlee\source\repos\tlee133\ATC VWorks Plugin\TestData\VWorks Device File\Multiple_ATC.dev' Display_User_Task_Descriptions='1' DynamicAssignPlateStorageLoad='0' FinishScript='' Form_File='' HandlePlatesInInstance='1' ImportInventory='0' InventoryFile='' Notes='' PipettePlatesInInstanceOrder='0' Protocol_Alias='' StartScript='' Use_Global_JS_Context='0' />
	<Processes >
		<Main_Processes >
			<Process >
				<Minimized >0</Minimized>
				<Task Name='ATC::CLOSE' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='14' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<DisabledDevices >
						<DisabledDevice >Thermo ATC - 2</DisabledDevice>
					</DisabledDevices>
					<BackupParameters >
						<BackupParameter Category='Task Description' Description='The number that indicates the position of the task in the protocol.' Hide_if='' Name='Task number' Script='' Scriptable='1' Style='1' Type='8' Units='' Value='1' Valuetodisplay='' />
						<BackupParameter Category='Task Description' Description='The description of the task.' Hide_if='' Name='Task description' Script='' Scriptable='1' Style='1' Type='1' Units='' Value='CLOSE (ATC)' Valuetodisplay='' />
						<BackupParameter Category='Task Description' Description='The option to use the default task description or provide your own description for the task.

Select the check box to use the default description. Clear the check box to provide your own description.' Hide_if='' Name='Use default task description' Script='' Scriptable='1' Style='0' Type='0' Units='' Value='1' Valuetodisplay='' />
					</BackupParameters>
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='1' />
						<Parameter Category='Task Description' Name='Task description' Value='CLOSE (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::START' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<DisabledDevices >
						<DisabledDevice >Thermo ATC - 2</DisabledDevice>
					</DisabledDevices>
					<BackupParameters >
						<BackupParameter Category='' Description='File Path of ATC Protocol' Hide_if='' Name='Protocol File Path' Script='' Scriptable='1' Style='0' Type='9' Units='' Value='C:\Users\tlee\source\repos\tlee133\ATC VWorks Plugin\TestData\ATC Protocols\ThirtyCycleTest.xml' Valuetodisplay='' />
						<BackupParameter Category='Task Description' Description='The number that indicates the position of the task in the protocol.' Hide_if='' Name='Task number' Script='' Scriptable='1' Style='1' Type='8' Units='' Value='2' Valuetodisplay='' />
						<BackupParameter Category='Task Description' Description='The description of the task.' Hide_if='' Name='Task description' Script='' Scriptable='1' Style='1' Type='1' Units='' Value='START (ATC)' Valuetodisplay='' />
						<BackupParameter Category='Task Description' Description='The option to use the default task description or provide your own description for the task.

Select the check box to use the default description. Clear the check box to provide your own description.' Hide_if='' Name='Use default task description' Script='' Scriptable='1' Style='0' Type='0' Units='' Value='1' Valuetodisplay='' />
					</BackupParameters>
					<Parameters >
						<Parameter Category='' Name='Protocol File Path' Value='C:\Users\tlee\source\repos\tlee133\ATC VWorks Plugin\TestData\ATC Protocols\ThirtyCycleTest.xml' />
						<Parameter Category='Task Description' Name='Task number' Value='2' />
						<Parameter Category='Task Description' Name='Task description' Value='START (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Task Name='ATC::OPEN' >
					<Devices >
						<Device Device_Name='Thermo ATC - 1' Location_Name='Default Location' />
						<Device Device_Name='Thermo ATC - 1' Location_Name='Nest' />
					</Devices>
					<Enable_Backup >0</Enable_Backup>
					<Task_Disabled >0</Task_Disabled>
					<Task_Skipped >0</Task_Skipped>
					<Has_Breakpoint >0</Has_Breakpoint>
					<Advanced_Settings >
						<Setting Name='Estimated time' Value='5.0' />
					</Advanced_Settings>
					<TaskScript Name='TaskScript' Value='' />
					<DisabledDevices >
						<DisabledDevice >Thermo ATC - 2</DisabledDevice>
					</DisabledDevices>
					<BackupParameters >
						<BackupParameter Category='Task Description' Description='The number that indicates the position of the task in the protocol.' Hide_if='' Name='Task number' Script='' Scriptable='1' Style='1' Type='8' Units='' Value='3' Valuetodisplay='' />
						<BackupParameter Category='Task Description' Description='The description of the task.' Hide_if='' Name='Task description' Script='' Scriptable='1' Style='1' Type='1' Units='' Value='OPEN (ATC)' Valuetodisplay='' />
						<BackupParameter Category='Task Description' Description='The option to use the default task description or provide your own description for the task.

Select the check box to use the default description. Clear the check box to provide your own description.' Hide_if='' Name='Use default task description' Script='' Scriptable='1' Style='0' Type='0' Units='' Value='1' Valuetodisplay='' />
					</BackupParameters>
					<Parameters >
						<Parameter Category='Task Description' Name='Task number' Value='3' />
						<Parameter Category='Task Description' Name='Task description' Value='OPEN (ATC)' />
						<Parameter Category='Task Description' Name='Use default task description' Value='1' />
					</Parameters>
				</Task>
				<Plate_Parameters >
					<Parameter Name='Plate name' Value='process - 1' />
					<Parameter Name='Plate type' Value='96 Greiner 655101 PS Clr Rnd Well Flat Btm' />
					<Parameter Name='Simultaneous plates' Value='1' />
					<Parameter Name='Plates have lids' Value='0' />
					<Parameter Name='Plates enter the system sealed' Value='0' />
					<Parameter Name='Use single instance of plate' Value='0' />
					<Parameter Name='Automatically update labware' Value='0' />
					<Parameter Name='Enable timed release' Value='0' />
					<Parameter Name='Release time' Value='30' />
					<Parameter Name='Auto managed counterweight' Value='0' />
					<Parameter Name='Barcode filename' Value='No Selection' />
					<Parameter Name='Has header' Value='' />
					<Parameter Name='Barcode or header South' Value='No Selection' />
					<Parameter Name='Barcode or header West' Value='No Selection' />
					<Parameter Name='Barcode or header North' Value='No Selection' />
					<Parameter Name='Barcode or header East' Value='No Selection' />
				</Plate_Parameters>
				<Quarantine_After_Process >0</Quarantine_After_Process>
			</Process>
		</Main_Processes>
	</Processes>
</Velocity11>