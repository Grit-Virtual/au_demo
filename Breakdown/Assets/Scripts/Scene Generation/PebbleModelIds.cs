using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PebbleModelIds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ProjectObject po = GetComponent<ProjectObject> ();
		switch (transform.root.name) {
		case "P-Plmb_L1_Area-A_Storm.nwd":
			po.modelId = "d01a0bf0-11b9-11e8-abb5-e12d052287d7";
			break;
		case "P-Plmb_L1_Area-A_Misc.nwd":
			po.modelId = "bdf608c0-11b9-11e8-a19f-23b1e20d8217";
			break;
		case "P-Plmb_L1_Area-A_Equip.nwd":
			po.modelId = "b32330d0-11b9-11e8-a19f-23b1e20d8217";
			break;
		case "P-Plmb_L1_Area-A_DWV.nwd":
			po.modelId = "926d03c0-11b9-11e8-a19f-23b1e20d8217";
			break;
		case "P-Plmb_L1_Area-A_DHW.nwd":
			po.modelId = "8723ea60-11b9-11e8-a19f-23b1e20d8217";
			break;
		case "P-Plmb_L1_Area-A_DCW.nwd":
			po.modelId = "79ddc880-11b9-11e8-abb5-e12d052287d7";
			break;
		case "M-Duct_L1_Area-A_Supply.nwd":
			po.modelId = "a1dccc20-11a8-11e8-b4dc-9d54ffcf8760";
			break;
		case "M-Duct_L1_Area-A_Return.nwd":
			po.modelId = "9c52cc50-11a8-11e8-b4dc-9d54ffcf8760";
			break;
		case "M-Duct_L1_Area-A_Exhaust.nwd":
			po.modelId = "9753f580-11a8-11e8-b4dc-9d54ffcf8760";
			break;
		case "M-Duct_L1_Area-A_Equip.nwd":
			po.modelId = "9049be50-11a8-11e8-b4dc-9d54ffcf8760";
			break;
		case "BarJoist_Area-A_L2_Roof.nwd":
			po.modelId = "c68291a0-11a2-11e8-8085-e70128538c22";
			break;
		case "BarJoist_Area-A_L1_Roof.nwd":
			po.modelId = "c1da6650-11a2-11e8-8085-e70128538c22";
			break;
		case "Elec_L2_Area-A_03.nwd":
			po.modelId = "d7adf030-1032-11e8-8422-bb6f9a4629ad";
			break;
		case "Elec_L1_Area-A_03.nwd":
			po.modelId = "493bdfb0-1032-11e8-8422-bb6f9a4629ad";
			break;
		case "Generic_Models.nwd":
			po.modelId = "7aab8570-101c-11e8-ada4-6dd93d7b6ae2";
			break;
		case "Ceilings_Area-A.nwd":
			po.modelId = "da1c9db0-101b-11e8-8422-bb6f9a4629ad";
			break;
		case "Fire_L1_Area-A.nwd":
			po.modelId = "6eba0f44-2583-4d50-9ffb-cec1f36ee31b";
			break;
		case "Walls_Area-A.nwd":
			po.modelId = "bf20c43e-be32-4398-b32a-8f200abacfe7";
			break;
		case "P-MGas_L1_Area-A.nwd":
			po.modelId = "41b75f6f-5612-4076-a11b-837807fba982";
			break;
		case "Foundations_Area-A.nwd":
			po.modelId = "0d2b80eb-d755-4d06-bd03-7f7801fcc76d";
			break;
		case "Floors_Decking_Area-A.nwd":
			po.modelId = "c4c692f4-a7f6-4eab-be6f-129776780883";
			break;
		case "P-Plmb_L2_Area-A.nwd":
			po.modelId = "8f1367d2-9fb0-4d0c-95c5-a3a1ec5b7862";
			break;
		case "M-Pipe_L1_Area-A.nwd":
			po.modelId = "a0d29e28-0e8b-4170-8fbd-4c5dcf8a792a";
			break;
		case "Steel_Area-A_04.nwd":
			po.modelId = "8eeeb972-393a-4aef-8fda-9e926a3c782a";
			break;
		case "M-Pipe_L2_Area-A.nwd":
			po.modelId = "409d53b6-4c42-4641-ae98-9a2f9eed1a4a";
			break;
		case "M-Duct_Roof_Area-A.nwd":
			po.modelId = "dbd44f44-46f3-401d-925b-e6b18df2f79d";
			break;
		case "Fire_L2_Area-A.nwd":
			po.modelId = "b995f258-07d0-4d1c-8582-67964bfbc80b";
			break;
		case "Roof_Area-A-02.nwd":
			po.modelId = "f838d0d6-e52a-4446-bebc-64d3eca269d0";
			break;
		}
	}
}
