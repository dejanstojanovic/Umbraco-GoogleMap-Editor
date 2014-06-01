using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.GoogleMaps.Models;

namespace Umbraco.GoogleMaps.DataType
{
    public class GoogleMapDataType : umbraco.cms.businesslogic.datatype.BaseDataType, umbraco.interfaces.IDataType
    {
        private umbraco.interfaces.IDataEditor _Editor;
        private umbraco.interfaces.IData _baseData;
        private GoogleMapPrevalueEditor _prevalueeditor;

        public override umbraco.interfaces.IData Data
        {
            get
            {
                if (_baseData == null)
                {
                    _baseData = new umbraco.cms.businesslogic.datatype.DefaultData(this);
                }
                else
                {
                    MapState state = _baseData as MapState;
                    if (state != null && state.Locations.Any())
                    {
                        _baseData = null;
                    }
                }
                return _baseData;
            }
        }

        public override umbraco.interfaces.IDataEditor DataEditor
        {
            get
            {
                if (_Editor == null)
                    _Editor = new GoogleMapEditor(Data, ((GoogleMapPrevalueEditor)PrevalueEditor).Configuration);
                return _Editor;
            }
        }

        public override string DataTypeName
        {
            get { return "GoogleMapDataType"; }
        }

        public override Guid Id
        {
            get { return new Guid(Constants.DATATYPE_GUID); }
        }

        public override umbraco.interfaces.IDataPrevalue PrevalueEditor
        {
            get
            {
                if (_prevalueeditor == null)
                    _prevalueeditor = new GoogleMapPrevalueEditor(this);
                return _prevalueeditor;
            }
        }
    }
}
