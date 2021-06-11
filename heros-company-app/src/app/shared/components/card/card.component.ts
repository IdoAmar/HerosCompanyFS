import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {

    @Input() id = '3F2504E0-4F89-11D3-9A0C-0305E82C3301';
    @Input() name = 'test';
    @Input() currentPower = '340';
    @Input() startingPower = '100';
    @Input() ability = 'fire';
    @Input() joinedAt = 'yesterday';
    @Input() suitColor = 'orange-red';

  constructor() { }

  ngOnInit(): void {
  }

}
