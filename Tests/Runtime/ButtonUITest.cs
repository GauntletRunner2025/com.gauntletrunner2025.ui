using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using NUnit.Framework;
using System.Collections;
using Unity.Entities;

namespace Gauntletrunner2025.Ui.Tests
{
    partial class TestButtonUI : ButtonUI
    {
        protected override string Name => "TestButton";
        protected override ElementSourceMode SourceMode => ElementSourceMode.ExistsInTree;
        protected override WaitModeEnum WaitMode => WaitModeEnum.Wait;
        protected override void Initialize(VisualElement root, Button element) { }
    }

    partial class TestButtonUI_DoNotWait : ButtonUI
    {
        protected override string Name => "TestButton";
        protected override ElementSourceMode SourceMode => ElementSourceMode.ExistsInTree;
        protected override WaitModeEnum WaitMode => WaitModeEnum.DoNotWait;
        protected override void Initialize(VisualElement root, Button element) { }
    }

    partial class TestButtonUI_Wait : ButtonUI
    {
        protected override string Name => "TestButton";
        protected override ElementSourceMode SourceMode => ElementSourceMode.ExistsInTree;
        protected override WaitModeEnum WaitMode => WaitModeEnum.Wait;
        protected override void Initialize(VisualElement root, Button element) { }
    }

    class ButtonUITest
    {
        private UIDocument uiDocument;
        private World testWorld;
        private TestButtonUI_Wait waitButtonSystem;
        private TestButtonUI_DoNotWait noWaitButtonSystem;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            testWorld = new World("TestWorld");
            waitButtonSystem = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<TestButtonUI_Wait>();
            noWaitButtonSystem = World.DefaultGameObjectInjectionWorld.CreateSystemManaged<TestButtonUI_DoNotWait>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (testWorld != null)
            {
                testWorld.Dispose();
            }
        }

        [SetUp]
        public void Setup()
        {
            // Create UI Document
            var go = new GameObject("UI");
            uiDocument = go.AddComponent<UIDocument>();
            var root = new VisualElement();
            uiDocument.rootVisualElement.Add(root);
        }

        [TearDown]
        public void TearDown()
        {
            if (uiDocument != null)
            {
                Object.DestroyImmediate(uiDocument.gameObject);
            }
        }

        [UnityTest]
        public IEnumerator ButtonUI_WhenElementIsAdded_HasValidElement()
        {
            // Start the system
            waitButtonSystem.Update();
            // Initially the element shouldn't be found
            Assert.IsNull(waitButtonSystem.Button);

            // Create and add the button
            var button = new Button { name = "TestButton" };
            uiDocument.rootVisualElement.Add(button);

            // Let the system try to find it
            waitButtonSystem.Update();

            // Now the button should be found
            Assert.IsNotNull(waitButtonSystem.Button);
            Assert.AreEqual(button, waitButtonSystem.Button);

            yield return null;
        }

        [UnityTest]
        public IEnumerator ButtonUI_WhenWaitModeEnabled_StaysEnabled()
        {
            // Start both systems
            waitButtonSystem.Update();
            noWaitButtonSystem.Update();

            // Both systems should be enabled initially
            Assert.IsTrue(waitButtonSystem.Enabled);
            Assert.IsTrue(noWaitButtonSystem.Enabled);

            // Let them try to find the non-existent button
            yield return null;
            waitButtonSystem.Update();
            noWaitButtonSystem.Update();

            // Wait mode system should stay enabled, DoNotWait should disable
            Assert.IsTrue(waitButtonSystem.Enabled);
            Assert.IsFalse(noWaitButtonSystem.Enabled);
        }

        [UnityTest]
        public IEnumerator ButtonUI_WhenElementIsRemovedAndReadded_ReconnectsElement()
        {
            // Create and add the initial button
            var button = new Button { name = "TestButton" };
            uiDocument.rootVisualElement.Add(button);

            // Let the system find it
            waitButtonSystem.Update();
            Assert.IsNotNull(waitButtonSystem.Button);

            // Remove the button
            button.RemoveFromHierarchy();
            waitButtonSystem.Update();
            Assert.IsNull(waitButtonSystem.Button);

            // Add a new button with same name
            var newButton = new Button { name = "TestButton" };
            uiDocument.rootVisualElement.Add(newButton);

            // System should find the new button
            waitButtonSystem.Update();
            Assert.IsNotNull(waitButtonSystem.Button);
            Assert.AreEqual(newButton, waitButtonSystem.Button);

            yield return null;
        }
    }
}