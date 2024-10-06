using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace WildWind.Core
{

    public class ObjectSwapper<T>
    {

        public delegate ref T ReturnObjectByRef();
        private ReturnObjectByRef originalObject;
        private ReturnObjectByRef alternativeObject;

        public ObjectSwapper(ReturnObjectByRef original, ReturnObjectByRef alternative, Func<bool> violationCondition, CancellationTokenSource cancellationTokenSource, ref Task task)
        {

            originalObject = original;
            alternativeObject = alternative;

            SwapObjects();

            task = ConditionViolation(violationCondition, cancellationTokenSource.Token);

        }

        private async Task ConditionViolation(Func<bool> violationCondition, CancellationToken token)
        {

            try
            {
                await Task.Run(() =>
                {
                    while (!(violationCondition() || token.IsCancellationRequested))
                    {

                    }
                });
            }
            finally
            {
                SwapObjects();
            }

        }

        private void SwapObjects()
        {

            T originalHelper = (T)((object)originalObject());
            originalObject() = (T)((object)alternativeObject());
            alternativeObject() = originalHelper;

        }

    }

}