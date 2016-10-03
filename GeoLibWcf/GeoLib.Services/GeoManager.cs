using System.Collections.Generic;
using System.Linq;
using GeoLib.Contracts;
using GeoLib.Data;

namespace GeoLib.Services
{
    public class GeoManager: IGeoService
    {
        private IZipCodeRepository _zipCodeRepository = null;
        private IStateRepository _stateRepository = null;

        public GeoManager()
        {
            //Not implemented
        }

        public GeoManager(IZipCodeRepository zipCodeRepository)
            : this(zipCodeRepository, null)
        {
        }

        public GeoManager(IStateRepository stateRepository)
            : this(null, stateRepository)
        {
        }

        public GeoManager(IZipCodeRepository zipCodeRepository, IStateRepository stateRepository)
        {
            _zipCodeRepository = zipCodeRepository;
            _stateRepository = stateRepository;
        }

        public ZipCodeData GetZipInfo(string zip)
        {
            ZipCodeData zipCodeData = null;

            IZipCodeRepository zipCodeRepository = _zipCodeRepository ?? new ZipCodeRepository();

            ZipCode zipCodeEntity = zipCodeRepository.GetByZip(zip);
            if (zipCodeEntity != null)
            {
                zipCodeData = new ZipCodeData
                {
                    City = zipCodeEntity.City,
                    State = zipCodeEntity.State.Abbreviation,
                    ZipCode = zipCodeEntity.Zip
                };
            }

            return zipCodeData;
        }

        public IEnumerable<string> GetStates(bool primaryOnly)
        {
            List<string> stateDatas = new List<string>();

            IStateRepository stateRepository = _stateRepository ?? new StateRepository();

            IEnumerable<State> states = stateRepository.Get(primaryOnly);
            if (states != null)
            {
                foreach (var state in states)
                {
                    stateDatas.Add(state.Abbreviation);
                }
            }

            return stateDatas;
        }

        public IEnumerable<ZipCodeData> GetZips(string state)
        {
            List<ZipCodeData> zipCodeDatas = new List<ZipCodeData>();

            IZipCodeRepository zipCodeRepository = _zipCodeRepository ?? new ZipCodeRepository();

            var zips = zipCodeRepository.GetByState(state);
            if (zips != null)
            {
                zipCodeDatas.AddRange(zips.Select(zipCode => new ZipCodeData
                {
                    City = zipCode.City, State = zipCode.State.Abbreviation, ZipCode = zipCode.Zip
                }));
            }

            return zipCodeDatas;
        }

        public IEnumerable<ZipCodeData> GetZips(string zip, int range)
        {
            List<ZipCodeData> zipCodeDatas = new List<ZipCodeData>();

            IZipCodeRepository zipCodeRepository = _zipCodeRepository ?? new ZipCodeRepository();

            ZipCode zipEntity = zipCodeRepository.GetByZip(zip);

            IEnumerable<ZipCode> zips = zipCodeRepository.GetZipsForRange(zipEntity, range);
            if (zips != null)
            {
                zipCodeDatas.AddRange(zips.Select(zipCode => new ZipCodeData
                {
                    City = zipCode.City,
                    State = zipCode.State.Abbreviation,
                    ZipCode = zipCode.Zip
                }));
            }

            return zipCodeDatas;
        }
    }
}
