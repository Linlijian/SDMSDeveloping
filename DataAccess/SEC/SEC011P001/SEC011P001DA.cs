using System.Data;
using System.Linq;
using UtilityLib;

namespace DataAccess.SEC
{
    public class SEC011P001DA : BaseDA
    {
        public SEC011P001DTO DTO { get; set; }

        #region ====Property========
        public SEC011P001DA()
        {
            DTO = new SEC011P001DTO();
        }

        #endregion

        #region ====Select==========
        protected override BaseDTO DoSelect(BaseDTO baseDTO)
        {
            var dto = (SEC011P001DTO)baseDTO;
            switch (dto.Execute.ExecuteType)
            {
                case SEC011P001ExecuteType.GetAll: break;
            }
            return dto;
        }
        
        #endregion

        #region ====Insert==========
        protected override BaseDTO DoInsert(BaseDTO baseDTO)
        {
            var dto = (SEC011P001DTO)baseDTO;
            
            return dto;
        }
        #endregion

        #region ====Update==========
        protected override BaseDTO DoUpdate(BaseDTO baseDTO)
        {
            var dto = (SEC011P001DTO)baseDTO;
           
            return dto;
        }
        #endregion

        #region ====Delete==========
        protected override BaseDTO DoDelete(BaseDTO baseDTO)
        {
            var dto = (SEC011P001DTO)baseDTO;

           
            return dto;
        }
        #endregion
    }
}