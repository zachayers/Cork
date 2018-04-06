
using System.Collections.Generic;

namespace Cork_ExternalUtils
{
    public class CorkLayerDictionary
    {        
        //Plywood Layers
        private readonly Material _plywoodBending = new Material("Plywood - Bending (.35)", 30);
        private readonly Material _plywoodRawpt25 = new Material("Plywood - Raw (.25)", 20);
        private readonly Material _plywoodRawpt50 = new Material("Plywood - Raw (.50)", 110);
        private readonly Material _plywoodRawpt75 = new Material("Plywood - Raw (.75)", 41);

        //Raw MDF Layers
        private readonly Material _mdFpt125 = new Material("MDF - Raw (.125)", 204);
        private readonly Material _mdFpt25 = new Material("MDF - Raw (.25)", 196);
        private readonly Material _mdFpt3125 = new Material("MDF - Raw (.3125)", 113);
        private readonly Material _mdFpt375 = new Material("MDF - Raw (.375)", 21);
        private readonly Material _mdFpt50 = new Material("MDF - Raw (.50)", 142);
        private readonly Material _mdFpt625 = new Material("MDF - Raw (.625)", 42);
        private readonly Material _mdFpt75 = new Material("MDF - Raw (.75)", 151);
        private readonly Material _mdf1Pt0 = new Material("MDF - Raw (1.0)", 112);

        //Post Lam MDF Layers
        private readonly Material _lamMdFpt5625Grain = new Material("MDF Laminated - xxx (.5625) - Grain", 55);
        private readonly Material _lamMdFpt8125Grain = new Material("MDF Laminated - xxx (.8125) - Grain", 14);
        private readonly Material _lamMdFpt875Grain = new Material("MDF Laminated - xxx (.875) - Grain", 33);
        private readonly Material _lamMdf1Pt125Grain = new Material("MDF Laminated - xxx (1.125) - Grain", 170);

        private readonly Material _lamMdFpt5625Solid = new Material("MDF Laminated - xxx (.5625) - Solid", 105);
        private readonly Material _lamMdFpt8125Solid = new Material("MDF Laminated - xxx (.8125) - Solid", 252);
        private readonly Material _lamMdFpt875Solid = new Material("MDF Laminated - xxx (.875) - Solid", 96);
        private readonly Material _lamMdf1Pt125Solid = new Material("MDF Laminated - xxx (1.125) - Solid", 174);

        //Post Lam Layers
        private readonly Material _postLaminateGrain = new Material("Post Laminate - xxx - Grain", 14);
        private readonly Material _postLaminateSolid = new Material("Post Laminate - xxx - Solid", 252);

        // PVC Edge Layers
        private readonly Material _pvcEdge1 = new Material("PVC Edge - xxx (.02) .5MM", 71);
        private readonly Material _pvcEdge2 = new Material("PVC Edge - xxx (.04) 1MM", 72);
        private readonly Material _pvcEdge3 = new Material("PVC Edge - xxx (.125) 3MM", 41);

        //Countertop Layers
        private readonly Material _countertop1 = new Material("Countertop - Dark (.50)", 185);
        private readonly Material _countertop2 = new Material("Countertop - Light (.50)", 253);

        //Steel Layers
        private readonly Material _steel1 = new Material("Steel Plate (.1875)", 124);
        private readonly Material _steel2 = new Material("Steel Plate (.125)", 14);
        private readonly Material _steel3 = new Material("Steel Plate (.25)", 141);
        private readonly Material _steel4 = new Material("Steel Plate - 11 Gauge", 85);
        private readonly Material _steel5 = new Material("Steel Plate - 12 Gauge", 225);
        private readonly Material _steel6 = new Material("Steel Plate - 13 Gauge", 202);
        private readonly Material _steel7 = new Material("Steel Plate - 14 Gauge", 136);
        private readonly Material _steel8 = new Material("Steel Plate - 16 Gauge", 145);
        private readonly Material _steel9 = new Material("Steel Plate - 18 Gauge", 84);
        private readonly Material _steel10 = new Material("Steel Plate - 20 Gauge", 54);

        private readonly Material _sSteel1 = new Material("S. Steel Plate (.1875)", 130);
        private readonly Material _sSteel2 = new Material("S. Steel Plate (.125)", 151);
        private readonly Material _sSteel3 = new Material("S. Steel Plate (.25)", 17);
        private readonly Material _sSteel4 = new Material("S. Steel Plate - 11 Gauge", 83);
        private readonly Material _sSteel5 = new Material("S. Steel Plate - 12 Gauge", 30);
        private readonly Material _sSteel6 = new Material("S. Steel Plate - 13 Gauge", 201);
        private readonly Material _sSteel7 = new Material("S. Steel Plate - 14 Gauge", 136);
        private readonly Material _sSteel8 = new Material("S. Steel Plate - 16 Gauge", 170);
        private readonly Material _sSteel9 = new Material("S. Steel Plate - 18 Gauge", 155);
        private readonly Material _sSteel10 = new Material("S. Steel Plate - 20 Gauge", 52);

        //Tube Layers
        private readonly Material _tube1 = new Material("Steel Tube - 11 Gauge", 193);
        private readonly Material _tube2 = new Material("Steel Tube - 13 Gauge", 120);
        private readonly Material _tube3 = new Material("Steel Tube - 14 Gauge", 132);
        private readonly Material _tube4 = new Material("Steel Tube - 16 Gauge", 140);


        //Init Layers
        public List<Material> InitLayers()
        {
            // Create Layer List
            return new List<Material>
            {
                _plywoodBending,
                _plywoodRawpt25,
                _plywoodRawpt50,
                _plywoodRawpt75,
                _mdFpt125,
                _mdFpt25,
                _mdFpt3125,
                _mdFpt375,
                _mdFpt50,
                _mdFpt625,
                _mdFpt75,
                _mdf1Pt0,
                _lamMdFpt5625Grain,
                _lamMdFpt8125Grain,
                _lamMdFpt875Grain,
                _lamMdf1Pt125Grain,
                _lamMdFpt5625Solid,
                _lamMdFpt8125Solid,
                _lamMdFpt875Solid,
                _lamMdf1Pt125Solid,
                _postLaminateGrain,
                _postLaminateSolid,
                _pvcEdge1,
                _pvcEdge2,
                _pvcEdge3,
                _countertop1,
                _countertop2,
                _steel1,
                _steel2,
                _steel3,
                _steel4,
                _steel5,
                _steel6,
                _steel7,
                _steel8,
                _steel9,
                _steel10,
                _sSteel1,
                _sSteel2,
                _sSteel3,
                _sSteel4,
                _sSteel5,
                _sSteel6,
                _sSteel7,
                _sSteel8,
                _sSteel9,
                _sSteel10,
                _tube1,
                _tube2,
                _tube3,
                _tube4
            };
        }
    }

    public class Material
    {
        public string Name;
        public short Color;

        public Material(string nme, short clr)
        {
            Name = nme;
            Color = clr;
        }
    }
}
