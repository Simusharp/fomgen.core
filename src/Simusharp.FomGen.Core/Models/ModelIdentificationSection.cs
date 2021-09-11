/*
 *   Copyright 2021 Simusharp
 *   Don't remove this header
 *   Distributed under the MIT License.
 */

using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Simusharp.FomGen.Core.Models
{
    public class ModelIdentificationSection : FomSection
    {
        private readonly List<Keyword> _keywords = new List<Keyword>();
        private readonly List<string> _releaseRestrictions = new List<string>();
        private readonly List<string> _useHistory = new List<string>();
        private readonly List<PointOfContact> _poc = new List<PointOfContact>();
        private readonly List<Reference> _references = new List<Reference>();

        public override string SectionName { get; } = "modelIdentification";

        internal override IEnumerable<ValidationFailure> Validate(IValidator<string> validator)
        {
            return Array.Empty<ValidationFailure>();
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Version { get; set; }

        public DateTime? ModificationDate { get; set; }

        public string SecurityClassification { get; set; }

        public string Purpose { get; set; } = null;

        public string ApplicationDomain { get; set; } = null;

        public string Description { get; set; }

        public string UseLimitation { get; set; } = null;

        public string Other { get; set; } = null;

        public Glyph Glyph { get; set; } = null;

        public IReadOnlyList<string> ReleaseRestrictions => this._releaseRestrictions;

        public IReadOnlyList<string> UseHistory => this._useHistory;

        public IReadOnlyList<Keyword> Keywords => this._keywords;

        public IReadOnlyList<PointOfContact> POC => this._poc;

        public IReadOnlyList<Reference> References => this._references;

        public void AddReleaseRestriction(string restriction)
        {
            this._releaseRestrictions.Add(restriction);
        }

        public void RemoveRestriction(string restriction)
        {
            if (this._releaseRestrictions.Contains(restriction))
            {
                this._releaseRestrictions.Remove(restriction);
            }
        }

        public void AddUseHistory(string useHistory)
        {
            this._useHistory.Add(useHistory);
        }

        public void RemoveUseHistory(string useHistory)
        {
            if (this._useHistory.Contains(useHistory))
            {
                this._useHistory.Remove(useHistory);
            }
        }

        public void AddKeyword(Keyword keyword)
        {
            this._keywords.Add(keyword);
        }

        public void RemoveKeyword(Keyword keyword)
        {
            if (this._keywords.Contains(keyword))
            {
                this._keywords.Remove(keyword);
            }
        }

        public void AddPoc(PointOfContact poc)
        {
            this._poc.Add(poc);
        }

        public void RemovePoc(PointOfContact poc)
        {
            if (this._poc.Contains(poc))
            {
                this._poc.Remove(poc);
            }
        }

        public void AddReference(Reference reference)
        {
            this._references.Add(reference);
        }

        public void RemoveReference(Reference reference)
        {
            if (this._references.Contains(reference))
            {
                this._references.Remove(reference);
            }
        }
    }
}
