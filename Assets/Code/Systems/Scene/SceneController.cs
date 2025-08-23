using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.SceneManagement
{
    public class SceneController : SingletonBasic<SceneController>
    {
        [SerializeField] private RectTransform _transform;
        [SerializeField] private FadeScene _fade;
        [SerializeField] private bool _backupScene;
        [SerializeField] private UnityEvent<bool> _onSceneChange;

        public List<string> scenes { get; protected set; }
        private static string _lastScene;
        private bool _lock;

        protected override void Awake() { base.Awake(); scenes = new(); }
        public void SwipeScene(string value) => OnFading(value);
        public void ReturnScene(string value) => OnFading(string.IsNullOrEmpty(_lastScene) ? value : _lastScene);
        public void Quit() => OnFading(string.Empty);

        public IEnumerator AddScene(string value)
        {
            if (scenes.Contains(value)) yield break;
            yield return SceneManager.LoadSceneAsync(value, LoadSceneMode.Additive);
            _onSceneChange.Invoke(false);
            scenes.Add(value);
        }
        public void RemoveScene(string value)
        {
            if (!scenes.Contains(value)) return;
            SceneManager.UnloadSceneAsync(value, UnloadSceneOptions.None);
            _onSceneChange.Invoke(true);
            scenes.Remove(value);
        }
        
        public void OnCutScene(string value)
        {
            _lastScene = _backupScene ? SceneManager.GetActiveScene().path : null;

            SceneManager.LoadSceneAsync(value, LoadSceneMode.Single);
        }
        private void OnFading(string value)
        {
            if (_lock || value == SceneManager.GetActiveScene().path) return;

            Instantiate(_fade, _transform).onComplete += onComplete;
            _lock = true;

            void onComplete(bool _)
            {
                Time.timeScale = 1;
                if (string.IsNullOrEmpty(value)) Application.Quit();
                else OnCutScene(value);
            }
        }
    }
}