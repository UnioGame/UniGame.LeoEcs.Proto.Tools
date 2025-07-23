namespace UniGame.LeoEcs.Behaviours
{
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using Proto;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class AddressablesEcsObjectInstance : MonoBehaviour
    {
        [SerializeField]
        private bool _createOnStart = true;
    
        [SerializeField] 
        private AssetReferenceGameObject _gameObjectReference;

        private CancellationTokenSource _source;
    
        public async UniTask CreateInstance()
        {
            await EcsTools.WaitWorldReady(_source.Token);
            
            var resource = await _gameObjectReference
                .LoadAssetAsync<GameObject>()
                .ToUniTask(cancellationToken:_source.Token);
            Instantiate(resource);
        }
    
        private void Start()
        {
            _source = new CancellationTokenSource();
            if(_createOnStart)
                CreateInstance().Forget();
        }

        private void OnDisable()
        {
            _source.Cancel();
            _source.Dispose();
        }
    }
}
